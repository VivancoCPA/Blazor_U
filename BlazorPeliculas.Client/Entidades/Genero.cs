using BlazorPeliculas.Client.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BlazorPeliculas.Client.Entidades;

public class Genero
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [PrimeraLetraMayuscula]
    public string? Nombre { get; set; }
    public List<GeneroPelicula> GenerosPelicula { get; set; } = [];//
}
