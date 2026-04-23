using BlazorPeliculas.Client.Servicios;
using BlazorPeliculas.Components;
using BlazorPeliculas.Components.Account;
using BlazorPeliculas.Constantes;
using BlazorPeliculas.Datos;
using BlazorPeliculas.Entidades;
using BlazorPeliculas.Politicas;
using BlazorPeliculas.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();
//
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
}).AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
})
  .AddIdentityCookies();
//politicas de autorizacion
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("PuedeEditarRoles", politica =>
    {
        //si el usuario es admin o tiene el claim de superadmin, entonces puede editar roles
        politica.RequireAssertion(contexto => 
        {
            return contexto.User.IsInRole(Roles.ROL_ADMIN) ||
                    contexto.User.HasClaim(c => c.Type == "superadmin");
        });
    });

    opciones.AddPolicy("PuedeEditarRolesDB", policy =>
    {
        policy.AddRequirements(new PuedeEditarRolesRequirement());
    });

});

builder.Services.AddScoped<IAuthorizationHandler, PuedeEditarRolesHandler>();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
//
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection")
    .UseSeeding((context, _) =>
    {
        var rolAdmin = "administrador";
        var roles = context.Set<IdentityRole>();
        var existeRolAdmin = roles.Any(r => r.Name == rolAdmin);

        if (!existeRolAdmin)
        {
            roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = rolAdmin,
                NormalizedName = rolAdmin.ToUpperInvariant()
            });

            context.SaveChanges();
        }
    }).UseAsyncSeeding(async (context, _, ct) =>
    {
        var rolAdmin = "administrador";

        var roles = context.Set<IdentityRole>();

        var existeRolAdmin = await roles.AnyAsync(r => r.Name == rolAdmin, ct);

        if (!existeRolAdmin)
        {
            roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = rolAdmin,
                NormalizedName = rolAdmin.ToUpperInvariant()
            });

            await context.SaveChangesAsync(ct);
        }
    }));

builder.Services.AddScoped<IServicioPeliculas, ServicioPeliculas>();
builder.Services.AddScoped<IServicioGeneros, ServicioGeneros>();
builder.Services.AddScoped<IServicioActores, ServicioActores>();
builder.Services.AddScoped<IServicioVotos,ServicioVoto>();

builder.Services.AddScoped<IServicioSeguridad, ServicioSeguridad>();
builder.Services.AddTransient<IEmailSender, ServicioCorreos>();

builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMudServices();


var app = builder.Build();
// Realiza las migraciones pendientes al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }
}
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
app.UseStaticFiles();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorPeliculas.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
