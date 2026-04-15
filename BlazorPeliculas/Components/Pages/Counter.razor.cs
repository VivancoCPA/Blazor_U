using Microsoft.JSInterop;

namespace BlazorPeliculas.Components.Pages;

public partial class Counter(
    ServiciosScoped serviciosScoped2,
    ServiciosSingleton serviciosSingleton,
    ServiciosTransient serviciosTransient,
    IJSRuntime JS
    )
{
    private int currentCount = 0;
    private static int currentCountStatic = 0;

    IJSObjectReference? moduleCounter;
    
    [JSInvokable]
    private async Task IncrementCount()
    {
        moduleCounter = await JS.InvokeAsync<IJSObjectReference>("import", "./js/counter.js");

        await moduleCounter.InvokeVoidAsync("mostrarAlerta", "vas a incrementar el contador" );

        currentCount++;
        currentCountStatic= currentCount;

        serviciosScoped2.Valor = currentCount;
        serviciosSingleton.Valor = currentCount;
        serviciosTransient.Valor = currentCount;

        await JS.InvokeVoidAsync("obtenerCurrentCount");
    }

    public async Task IncrementCountJavaScript()
    { 
        await JS.InvokeVoidAsync("invocarIncrementCount", DotNetObjectReference.Create(this) );
    }


    [JSInvokable]
    public static Task<int> ObtenerCurrentCount()
    {
        return Task.FromResult(currentCountStatic);
    }
}
