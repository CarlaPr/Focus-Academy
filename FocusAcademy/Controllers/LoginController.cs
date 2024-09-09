using System;
using FocusAcademy.Models;
using FocusAcademy.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace FocusAcademy.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public LoginController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmail(loginModel.Email);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            return RedirectToAction("Index", "AreaAluno");
                        }
                        TempData["MensagemErro"] = $"Houve um erro no login. Senha inválida";
                    }
                    else
                    {
                        TempData["MensagemErro"] = $"Houve um erro no login. Usuário e/ou senha inválido(s)";
                    }
                }

                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Houve um erro no login: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
