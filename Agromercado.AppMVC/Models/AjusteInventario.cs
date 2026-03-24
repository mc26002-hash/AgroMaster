using System;
using System.Collections.Generic;

namespace Agromercado.AppMVC.Models;

public partial class AjusteInventario
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public DateTime? Fecha { get; set; }

    public int Cantidad { get; set; }

    public string? Motivo { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
