using System;

public class Ejemplar
{

    public int Id { get; set; }
    public int LibroId { get; set; }
    public Libro Libro { get; set; }

    public string CodigoInterno { get; set; } = "";
    public string Estado { get; set; } = "DISPONIBLE";
}
