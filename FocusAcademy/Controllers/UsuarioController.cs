using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FocusAcademy.Controllers
{
    public class UsuarioController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Cadastro()
        {
            return View();
        }
        public IActionResult ListaUsuario()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}