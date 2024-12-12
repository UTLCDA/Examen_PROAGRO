using System;
using System.Collections.Generic;

namespace Examen_PROAGRO.Models;

public partial class Restaurante
{
    public string FsqId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? FormattedAddress { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public string? Foto { get; set; }

    public decimal? Rating { get; set; }
}
