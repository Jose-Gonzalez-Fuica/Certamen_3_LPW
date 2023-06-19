using Certamen_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Certamen_3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EtiquetaController : ControllerBase
    {
        private readonly GestionGastosContext db = new();
        [HttpGet]
        public async Task<IActionResult> GetEtiqueta()
        {
            Response response = new();
            try
            {
                if (db.Etiqueta == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var etiquetas = await db.Etiqueta.Select(
                                       x => new
                                       {
                                           x.Id,
                                           x.Etiqueta,
                                           x.Descripcion,
                                           x.Color,
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
        public async Task<IActionResult> GetEtiqueta(int id)
        {
            Response response = new();
            try
            {
                var buscarEtiqueta = await db.Etiqueta.FindAsync(id);
                if (buscarEtiqueta == null)
                {
                    response.Message = "No existe registros con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarEtiqueta;
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
        public async Task<ActionResult<Etiquetum>> PostEtiqueta(string etiqueta, string descripcion,
            string color)
        {
            Etiquetum etiquetaObj = new();
            etiquetaObj.Etiqueta = etiqueta;
            etiquetaObj.Descripcion = descripcion;
            etiquetaObj.Color = color;
            db.Etiqueta.Add(etiquetaObj);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";
            return Ok(response);

            //return CreatedAtAction("GetEtiqueta", new { id = etiquetaObj.Id }, etiquetaObj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEtiqueta(int id)
        {
            Response response = new();
            var buscarEtiqueta = await db.Etiqueta.FindAsync(id);
            if (buscarEtiqueta != null)
            {
                var movimientos = await db.Movimientos.FirstOrDefaultAsync(x => x.IdEtiqueta == id);
                if (movimientos != null)
                {

                    response.Message = "No se puede eliminar la etiqueta por tener movimientos";
                    return Ok(response);
                }
                else
                {
                    db.Remove(buscarEtiqueta);
                    await db.SaveChangesAsync();
                    response.Message = "La Etiqueta se ha eliminado con éxito";
                    response.Success = true;
                    return Ok(response);
                }
              
            }
            response.Message = "No se encuntra el id";
            return Ok(response);
        }


        [HttpPut]
        public async Task<ActionResult<Etiquetum>> PutEtiqueta(int id,string? etiqueta, string? descripcion,
            string? color)
        {
            Response response = new();
            var etiquetaObj = await db.Etiqueta.FindAsync(id);
            if(etiquetaObj != null)
                {
                if(etiqueta != null)
                etiquetaObj.Etiqueta = etiqueta;
                if(descripcion != null)
                etiquetaObj.Descripcion = descripcion;
                if(color != null)
                etiquetaObj.Color = color;
                await db.SaveChangesAsync();
                response.Success = true;
                response.Message = "Actualizado con éxito";
            }
            else
                {
                response.Message = "No se encuntra el id";
                return NotFound(response);

            }
            

            return CreatedAtAction("GetEtiqueta", new { id = etiquetaObj.Id }, etiquetaObj);
        }
    }
}
