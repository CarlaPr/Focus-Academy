using Microsoft.EntityFrameworkCore;
using FocusAcademy.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext ao contêiner de serviços
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura o pipeline de solicitação HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
