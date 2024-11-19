using System;
using FocusAcademy.Models;
using FocusAcademy.Data;
using FocusAcademy.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FocusAcademy.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public LoginController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        // Exibe a página de login
        public IActionResult Index()
        {
            return View();
        }

        // Processa o login
        [HttpPost]
        public IActionResult Entrar(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            // Valida o usuário no banco de dados
            UsuarioModel usuario = _usuarioRepositorio.ValidarLogin(model.Email, model.Senha);

            if (usuario != null)
            {
                // Armazena o ID do usuário na sessão
                HttpContext.Session.SetInt32("UserId", usuario.Id);
                HttpContext.Session.SetString("UserName", usuario.Nome);

                // Redireciona para a área do aluno
                return RedirectToAction("Index", "AreaAluno");

            }
            else
            {
                TempData["MensagemErro"] = "Email ou senha inválidos. Tente novamente.";
                return RedirectToAction("Index");
            }
        }

        // Realiza o logout
        public IActionResult Sair()
        {
            // Limpa os dados da sessão
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
