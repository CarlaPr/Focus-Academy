using FocusAcademy.Data;

var builder = WebApplication.CreateBuilder(args);

//serviços necessários
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<UsuarioRepositorio>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Tempo de expiração
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication("CookieAuthentication")
    .AddCookie("CookieAuthentication", options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
