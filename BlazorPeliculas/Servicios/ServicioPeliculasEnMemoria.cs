using BlazorPeliculas.Entidades;

namespace BlazorPeliculas.Servicios
{
    public class ServicioPeliculasEnMemoria:IServicioPeliculas
    {
        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>
            {
                new Pelicula {id=1, Titulo = "Captain America: Brave New World", FechaLanzamiento = new DateTime(2025, 2, 14)},
                new Pelicula {id=2, Titulo = "Mission Impossible - Deac Reackoning Part Two", FechaLanzamiento = new DateTime(2025, 5, 23)},
                new Pelicula {id=3, Titulo = "Avengers: Secrets Wars", FechaLanzamiento = new DateTime(2026, 12, 13)},
                new Pelicula {id=4, Titulo = "The Batman - Part II", FechaLanzamiento = null}
            };
        }
    }
}
