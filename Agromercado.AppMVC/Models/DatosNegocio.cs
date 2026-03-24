using System;
using System.Collections.Generic;

namespace Agromercado.AppMVC.Models;

public partial class DatosNegocio
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }
}
