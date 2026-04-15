namespace BlazorPeliculas.Entidades;

public class Pelicula
{
    public int id { get; set; }
    public required string Titulo { get; set; }
    public DateTime? FechaLanzamiento { get; set; }
}
