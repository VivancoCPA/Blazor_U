using BlazorPeliculas;
using BlazorPeliculas.Components;
using BlazorPeliculas.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<ServiciosTransient>();
builder.Services.AddScoped<ServiciosScoped>();
builder.Services.AddSingleton<ServiciosSingleton>();

builder.Services.AddScoped<IServicioPeliculas, ServicioPeliculasEnMemoria>();

//builder.Services.AddCascadingValue(sp => new AppState());// Agrega AppState como un servicio de cascada
builder.Services.AddScoped<AppStateService>(); // Agrega AppStateService como un servicio de ámbito (scoped)
builder.Services.AddCascadingValue(options =>
{
    var state =  options.GetRequiredService<AppStateService>();// Obtiene el servicio AppStateService
    return state.Source; // Devuelve la fuente de valor de cascada del estado   
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
