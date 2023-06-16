using System;
using System.Collections.Generic;

namespace Certamen_3.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public int IdGrupo { get; set; }

    public bool Habilitada { get; set; }

    public int? MontoEstimacion { get; set; }

    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
