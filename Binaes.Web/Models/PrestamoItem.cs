using System;

public class PrestamoItem
{
    public long Id { get; set; }

    public long PrestamoId { get; set; }
    public Prestamo Prestamo { get; set; }

    public int EjemplarId { get; set; }
    public Ejemplar Ejemplar { get; set; }

    public int Renovaciones { get; set; } = 0;
    public DateTime? FechaDevolucion { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}
