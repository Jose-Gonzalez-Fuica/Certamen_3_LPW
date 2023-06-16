using System;
using System.Collections.Generic;

namespace Certamen_3.Models;

public partial class Movimiento
{
    public int Id { get; set; }

    public string Tipo { get; set; } = null!;

    public string NombreDescriptivo { get; set; } = null!;

    public int Monto { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Imagen { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string? Observacion { get; set; }

    public int IdCategoria { get; set; }

    public int IdEtiqueta { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual Etiquetum IdEtiquetaNavigation { get; set; } = null!;
}
