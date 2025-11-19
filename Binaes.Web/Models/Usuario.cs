using System;

public class Usuario
{
    public int Id { get; set; }
    public string Carnet { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;
}
