using Certamen_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Certamen_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private readonly GestionGastosContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            Response response = new();
            try
            {
                if (db.Categoria == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var categorias = await db.Categoria.Select(
                                       x => new
                                       {
                                           x.Id,
                                           x.Nombre,
                                           x.Descripcion,
                                           x.FechaCreacion,
                                           x.IdGrupo,
                                           Grupo=x.IdGrupoNavigation.Nombre,
                                           x.Habilitada,
                                           x.MontoEstimacion,
                                       }).ToListAsync();
                if (categorias != null)
                {
                    if (categorias.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = categorias;
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
        public async Task<IActionResult> GetCategorias(int id)
        {
            Response response = new();
            try
            {
                var buscarCategoria = await db.Categoria.FindAsync(id);
                if (buscarCategoria == null)
                {
                    response.Message = "No existe registros con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarCategoria;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            Response response = new();
            var buscarCategoria = await db.Categoria.FindAsync(id);
            if (buscarCategoria != null)
            {
                var movimientos = await db.Movimientos.FirstOrDefaultAsync(x => x.IdCategoria == id);
                if (movimientos != null)
                {
                    response.Message = "No se puede eliminar la categoria por tener movimientos";
                    return Ok(response);
                }
                else
                {
                    db.Remove(buscarCategoria);
                    await db.SaveChangesAsync();
                    response.Message = "la categoria se ha eliminado con éxito";
                    response.Success = true;
                    return Ok(response);
                }

            }
            response.Message = "No se encuntra el id";
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Categorium>> PostCategoria(string nombre, string descripcion,
            int id_grupo, bool habilitada, int montoEstimacion)
        {
            Categorium categoriaObj = new();
            categoriaObj.Nombre = nombre;
            categoriaObj.Descripcion = descripcion;
            categoriaObj.FechaCreacion = DateTime.Now;
            categoriaObj.IdGrupo = id_grupo;
            categoriaObj.Habilitada = habilitada;
            categoriaObj.MontoEstimacion = montoEstimacion;
            db.Categoria.Add(categoriaObj);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";

            return Ok(response);
            //return CreatedAtAction("GetUsuario", new { id = categoriaObj.Id }, categoriaObj);
        }

        [HttpPut]
        public async Task<ActionResult<Categorium>> PutCategoria(int id, string? nombre, string? descripcion,
           DateTime? fechaCreacion, int? id_grupo, bool? habilitada, int? montoEstimacion)
        {
            Response response = new();
            var categoriaObj = await db.Categoria.FindAsync(id);
            if (categoriaObj != null)
            {
                if (nombre != null)
                    categoriaObj.Nombre = nombre;
                if (descripcion != null)
                    categoriaObj.Descripcion = descripcion;
                if (fechaCreacion != null)
                    categoriaObj.FechaCreacion = (DateTime)fechaCreacion;
                if(id_grupo != null)
                    categoriaObj.IdGrupo = (int)id_grupo;
                if (habilitada != null)
                    categoriaObj.Habilitada = (bool)habilitada;
                if (montoEstimacion != null)
                    categoriaObj.MontoEstimacion = (int)montoEstimacion;
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
            //return CreatedAtAction("GetUsuario", new { id = categoriaObj.Id }, categoriaObj);
        }

    }
}
