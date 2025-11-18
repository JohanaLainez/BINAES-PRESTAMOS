using System;
using System.Collections.Generic;

public class Prestamo
{
    public long Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int StaffId { get; set; }
    public Staff Staff { get; set; }

    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public string Estado { get; set; } = "ACTIVO";

    public List<PrestamoItem> Items { get; set; } = new();
}
