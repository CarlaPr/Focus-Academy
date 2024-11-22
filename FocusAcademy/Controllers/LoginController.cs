using FocusAcademy.Models;
using FocusAcademy.Data;
using FocusAcademy.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FocusAcademy.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public LoginController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Entrar(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            UsuarioModel usuario = _usuarioRepositorio.ValidarLogin(model.Email, model.Senha);

            if (usuario != null)
            {
                // Armazena o ID do usuário na sessão
                HttpContext.Session.SetInt32("UserId", usuario.Id);
                HttpContext.Session.SetString("UserName", usuario.Nome);

                return RedirectToAction("Index", "AreaAluno");

            }
            else
            {
                TempData["MensagemErro"] = "Email ou senha inválidos. Tente novamente.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
