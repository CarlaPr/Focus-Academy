using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Models;

namespace FocusAcademy.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        //pegar dados da model instanciando a homemodel
        HomeModel home = new HomeModel();
        home.Nome = "Carla"; //posteriormente substitituir o carla para pegar o nome da sessao do login
        home.Email = "Carla.com";
        return View(home);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
