using System;

public class Libro
{

    public int Id { get; set; }
    public string ISBN { get; set; } = "";
    public string Titulo { get; set; } = "";
    public int? AnioPublicacion { get; set; }

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
}
