using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Data;

namespace FocusAcademy.Controllers;

public class HomeController : Controller
{
    private readonly UsuarioRepositorio _repositorio;

    public HomeController(IConfiguration configuration)
    {
        _repositorio = new UsuarioRepositorio(configuration);
    }
    public IActionResult Index()
    {
       return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
