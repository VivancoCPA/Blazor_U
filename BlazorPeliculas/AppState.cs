using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas;

public class AppState
{
    public string Color { get; set; } = "violet";
}

public class AppStateService
{
    private AppState _appState { get; set; } = new();
    public CascadingValueSource<AppState> Source { get; }

    public AppStateService()
    {
        Source = new CascadingValueSource<AppState>(_appState, isFixed:false); //valor cascada que se puede cambiar
    }

    public Task NotifyChangedAsync()=> Source.NotifyChangedAsync(); // Método para notificar a los componentes que el estado ha cambiado
}