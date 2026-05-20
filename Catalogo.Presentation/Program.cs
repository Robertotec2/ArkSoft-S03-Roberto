using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Ruta del archivo JSON — Corregida a la carpeta "Add" que tienes en tu explorador
string jsonPath = Path.Combine(builder.Environment.ContentRootPath, "Add", "items.json");

// Registrar el repositorio JSON como implementación de IItemRepository
builder.Services.AddSingleton<IItemRepository>(new JsonItemRepository(jsonPath));

// Registrar el servicio de Application
builder.Services.AddScoped<ItemService>();

// Servicio requerido para que funcione app.UseAuthorization()
builder.Services.AddAuthorization();

// Servicio obligatorio para que el proyecto reconozca tus Controladores y Vistas (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// 💡 CLAVE: Esto le permite a .NET renderizar los estilos CSS, fondos y animaciones de tu vista
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();