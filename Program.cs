using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilitar sesiones

var app = builder.Build();

// Manejo de errores y seguridad
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware del pipeline HTTP
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        // Habilitar middleware de sesión
app.UseAuthorization();  // Autorización (si luego agregás autenticación)

// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
