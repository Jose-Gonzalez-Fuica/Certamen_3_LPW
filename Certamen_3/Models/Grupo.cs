using System;
using System.Collections.Generic;

namespace Certamen_3.Models;

public partial class Grupo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public int IdUsuario { get; set; }

    public virtual ICollection<Categorium> Categoria { get; set; } = new List<Categorium>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
