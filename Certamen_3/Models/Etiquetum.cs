using System;
using System.Collections.Generic;

namespace Certamen_3.Models;

public partial class Etiquetum
{
    public int Id { get; set; }

    public string Etiqueta { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Color { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
