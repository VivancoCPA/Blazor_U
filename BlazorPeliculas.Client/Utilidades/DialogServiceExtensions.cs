using BlazorPeliculas.Client.Components.Compartidos;
using MudBlazor;

namespace BlazorPeliculas.Client.Utilidades;

public static class DialogServiceExtensions
{
    public static async Task ShowAlerta(this IDialogService dialogService, string mensaje, bool esExito = true)
    {
        var options = new DialogOptions
        {
            NoHeader = true,
            MaxWidth = MaxWidth.ExtraSmall, 
            FullWidth = true,
            CloseOnEscapeKey = true,
        };

        var parameters = new DialogParameters<ConfirmacionModal>
    {
        { x => x.mainMessage, mensaje },
        { x => x.IsSuccess, esExito }
    };

        // El título va vacío "" porque usamos NoHeader = true
        await dialogService.ShowAsync<ConfirmacionModal>("", parameters, options);
    }
}
