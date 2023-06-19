using Certamen_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Certamen_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly GestionGastosContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetMovimientos()
        {
            Response response = new();
            try
            {
                if (db.Movimientos == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var movimientos = await db.Movimientos.Select(
                                       x => new
                                       {
                                           x.Id,
                                           x.Tipo,
                                           x.NombreDescriptivo,
                                           x.Monto,
                                           x.Descripcion,
                                           x.Imagen,
                                           x.FechaRegistro,
                                           x.Observacion,
                                           x.IdCategoria,
                                           Categoria=x.IdCategoriaNavigation.Nombre,
                                           x.IdEtiquetaNavigation.Etiqueta,
                                           x.IdEtiqueta
                                       }).ToListAsync();
                if (movimientos != null)
                {
                    if (movimientos.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = movimientos;
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
        public async Task<IActionResult> GetMovimientos(int id)
        {
            Response response = new();
            try
            {
                var buscarGrupo = await db.Movimientos.FindAsync(id);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            Response response = new();
            var buscarMovimiento = await db.Movimientos.FindAsync(id);
            if (buscarMovimiento != null)
            {
                db.Remove(buscarMovimiento);
                await db.SaveChangesAsync();
                response.Message = "El movimiento se ha eliminado con éxito";
                response.Success = true;
                return Ok(response);

            }
            response.Message = "No se encuntra el id";
            return NotFound(response);
        }


        [HttpPost]
        public async Task<ActionResult<Movimiento>> PostMovimiento(string tipo,string nombreDescriptivo,int monto,string descripcion,
            string observacion,int id_categoria, int id_etiqueta)
        {
            Movimiento movimentoObj = new();
            movimentoObj.Tipo = tipo;
            movimentoObj.NombreDescriptivo = nombreDescriptivo;
            movimentoObj.Monto = monto;
            movimentoObj.Descripcion = descripcion;
            //movimentoObj.Imagen = imagen;
            movimentoObj.FechaRegistro = DateTime.Now;
            movimentoObj.Observacion = observacion;
            movimentoObj.IdCategoria = id_categoria;
            movimentoObj.IdEtiqueta = id_etiqueta;
            db.Movimientos.Add(movimentoObj);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";

            return Ok(response);
            //return CreatedAtAction("GetUsuario", new { id = movimentoObj.Id }, movimentoObj);
        }


        [HttpPut]
        public async Task<ActionResult<Categorium>> PutMovimento(int id, string? tipo, string? nombreDescriptivo, int? monto, string? descripcion, string? imagen,
            DateTime? fechaRegistro,string? observacion, int? id_categoria, int? id_etiqueta)
        {
            Response response = new();
            var movimientoObj = await db.Movimientos.FindAsync(id);
            if (movimientoObj != null)
            {
              if(tipo != null)
                {
                    movimientoObj.Tipo = tipo;
                }
                if (nombreDescriptivo != null)
                {
                    movimientoObj.NombreDescriptivo = nombreDescriptivo;
                }
                if (monto != null)
                {
                    movimientoObj.Monto = (int)monto;
                }
                if (descripcion != null)
                {
                    movimientoObj.Descripcion = descripcion;
                }
                if (imagen != null)
                {
                    movimientoObj.Imagen = imagen;
                }
                if (fechaRegistro != null)
                {
                    movimientoObj.FechaRegistro = (DateTime)fechaRegistro;
                }
                if (observacion != null)
                {
                    movimientoObj.Observacion = observacion;
                }
                if (id_categoria != null)
                {
                    movimientoObj.IdCategoria = (int)id_categoria;
                }
                if (id_etiqueta != null)
                {
                    movimientoObj.IdEtiqueta = (int)id_etiqueta;
                }
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
            //return CreatedAtAction("GetUsuario", new { id = movimientoObj.Id }, movimientoObj);
        }

    }
}
