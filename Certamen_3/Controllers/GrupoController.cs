using Certamen_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace Certamen_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        private readonly GestionGastosContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetGrupos()
        {
            Response response = new();
            try
            {
                if (db.Grupos == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var grupos = await db.Grupos.Select(
                                       x => new
                                       {
                                           x.Id,
                                           x.Nombre,
                                           x.Descripcion,
                                           x.FechaCreacion,
                                           x.IdUsuario,
                                           x.IdUsuarioNavigation.NombreUsuario,
                                       }).ToListAsync();
                if (grupos != null)
                {
                    if (grupos.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = grupos;
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
        public async Task<IActionResult> GetGrupos(int id)
        {
            Response response = new();
            try
            {
                var buscarGrupo = await db.Grupos.FindAsync(id);
                if (buscarGrupo == null)
                {
                    response.Message = "No existe registros con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarGrupo;
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
        public async Task<ActionResult<Grupo>> PostGrupos(string nombre, string descripcion)
        {

            try
            {
                var usuario = db.Usuarios.FirstOrDefault();
                Response response = new();

                if (usuario != null)
                {
                    Grupo grupoObj = new();
                    grupoObj.Nombre = nombre;
                    grupoObj.Descripcion = descripcion;
                    grupoObj.FechaCreacion = DateTime.Now;
                    grupoObj.IdUsuario = usuario.Id;
                    db.Grupos.Add(grupoObj);
                    await db.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Guardado con éxito";

                    //return CreatedAtAction("GetUsuario", new { id = grupoObj.Id }, grupoObj);
                }
                else
                {
                    response.Success = false;
                    response.Message = "No hay Usuarios registrados";

                }

                return Ok(response);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupo(int id)
        {
            Response response = new();
            var buscarGrupos = await db.Grupos.FindAsync(id);
            if (buscarGrupos != null)
            {
                var categorias = await db.Categoria.FirstOrDefaultAsync(x => x.IdGrupo == id);
                if (categorias != null)
                {
                    response.Message = "No se puede eliminar el grupo por tener categorias";
                    return Ok(response);
                }
                else
                {
                    db.Remove(buscarGrupos);
                    await db.SaveChangesAsync();
                    response.Message = "El grupo se ha eliminado con éxito";
                    response.Success = true;
                    return Ok(response);
                }

            }
            response.Message = "No se encuntra el id";
            return Ok(response);
        }


        [HttpPut]
        public async Task<ActionResult<Grupo>> PutUsuario(int id, string? nombre, string? descripcion,
           DateTime? fechaCreacion, int? id_usuario)
        {
            Response response = new();
            var grupoObj = await db.Grupos.FindAsync(id);
            if (grupoObj != null)
            {
                if (nombre != null)
                    grupoObj.Nombre = nombre;
                if(descripcion != null)
                    grupoObj.Descripcion = descripcion;
                if(fechaCreacion != null)
                    grupoObj.FechaCreacion = (DateTime)fechaCreacion;
                if(id_usuario != null)
                    grupoObj.IdUsuario = (int)id_usuario;
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
            //return CreatedAtAction("GetUsuario", new { id = grupoObj.Id }, grupoObj);
        }
    }
}
