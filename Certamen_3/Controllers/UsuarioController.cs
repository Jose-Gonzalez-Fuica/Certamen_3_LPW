using Certamen_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Certamen_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly GestionGastosContext db = new();
        [HttpGet]
        public async Task<IActionResult> GetUsuario()
        {
            Response response = new();
            try
            {
                if (db.Usuarios == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var etiquetas = await db.Usuarios.Select(
                                       x => new
                                       {
                                           x.Id,
                                           x.NombreUsuario,
                                           x.Contrasena,
                                           x.Telefono,
                                           x.Correo,
                                           x.Apellidos,
                                           x.Nombres,
                                           x.MontoBilletera
                                       }).ToListAsync();
                if (etiquetas != null)
                {
                    if (etiquetas.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = etiquetas;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            Response response = new();
            try
            {
                var buscarUsuario = await db.Usuarios.FindAsync(id);
                if (buscarUsuario == null)
                {
                    response.Message = "No existe registros con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarUsuario;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(string nombreUsuario, string contrasena,
           int? telefono, string correo, string apellidos, string nombres, int montoBilletera)
        {
            Usuario usuarioObj = new();
            usuarioObj.NombreUsuario = nombreUsuario;
            usuarioObj.Contrasena = contrasena;
            usuarioObj.Telefono = telefono;
            usuarioObj.Correo= correo;
            usuarioObj.Apellidos= apellidos;
            usuarioObj.Nombres= nombres;
            usuarioObj.MontoBilletera= montoBilletera;
            db.Usuarios.Add(usuarioObj);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";

            return Ok(response);
            //return CreatedAtAction("GetUsuario", new { id = usuarioObj.Id }, usuarioObj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            Response response = new();
            var buscarUsuarios = await db.Usuarios.FindAsync(id);
            if (buscarUsuarios != null)
            {
                var grupos = await db.Grupos.FirstOrDefaultAsync(x=>x.IdUsuario == id);
                if(grupos != null)
                {
                    response.Message = "No se puede eliminar el usuario por tener grupos";
                    return Ok(response);
                }
                else
                {
                    db.Remove(buscarUsuarios);
                    await db.SaveChangesAsync();
                    response.Message = "El usuario se ha eliminado con éxito";
                    response.Success = true;
                    return Ok(response);
                }
               
            }
            response.Message = "No se encuntra el id";
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Usuario>> PutUsuario(int id, string? nombreUsuario, string? contrasena,
           int? telefono, string? correo, string? apellidos, string? nombres, int? montoBilletera)
        {
            Response response = new();
            var usuarioObj = await db.Usuarios.FindAsync(id);
            if (usuarioObj != null)
            {
                if (nombreUsuario != null)
                    usuarioObj.NombreUsuario = nombreUsuario;
                if (contrasena != null)
                    usuarioObj.Contrasena = contrasena;
                if (telefono != null)
                    usuarioObj.Telefono = telefono;
                if (correo != null)
                    usuarioObj.Correo = correo;
                if (apellidos != null)
                    usuarioObj.Apellidos = apellidos;
                if(nombres != null)
                    usuarioObj.Nombres = nombres;
                if (montoBilletera != null)
                    usuarioObj.MontoBilletera = (int)montoBilletera;
                await db.SaveChangesAsync();
                response.Success = true;
                response.Message = "Actualizado con éxito";
            }
            else
            {
                response.Message = "No se encuntra el id";
                return NotFound(response);

            }

            return Ok(response);
            //return CreatedAtAction("GetUsuario", new { id = usuarioObj.Id }, usuarioObj);
        }


    }
}
