namespace BlazorPeliculas.DTOs
{
    public class PaginacionDTO(int Pagina = 1, int RegistrosPorPagina = 10)
    {
        private const int _cantidadMaximaRegistrosPorPagina = 50;

        public int Pagina { get; init; } = Math.Max(1, Pagina);
        public int RegistrosPorPagina { get; init; } =
            Math.Clamp(RegistrosPorPagina, 1, _cantidadMaximaRegistrosPorPagina);
        //Math.Clamp() es un método que limita un valor dentro de un rango especificado. En este caso, se asegura de que RegistrosPorPagina esté entre 1 y _cantidadMaximaRegistrosPorPagina (50).

    }
}
