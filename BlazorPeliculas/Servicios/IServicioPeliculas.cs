using BlazorPeliculas.DTOs;
using BlazorPeliculas.Entidades;

namespace BlazorPeliculas.Servicios
{
    public interface IServicioPeliculas
    {
        Task<HomeDTO> ObtenerPeliculasHome();
        Task<int> Crear(Pelicula pelicula);
        Task Actualizar(Pelicula pelicula);
        Task<bool> Borrar(int id);
        Task<PeliculaDetalleDTO>? ObtenerDetalle(int id);
        Task<ResultadoPaginadoDTO<Pelicula>> Buscar(ParametrosBusquedaPeliculaDTO parametros);
        Task<EditarPeliculaDTO?> ObtenerEditarPelicula(int id);
    }
}
