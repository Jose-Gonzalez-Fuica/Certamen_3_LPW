using System;
using System.Collections.Generic;

namespace Certamen_3.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int? Telefono { get; set; }

    public string Correo { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public int MontoBilletera { get; set; }

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
