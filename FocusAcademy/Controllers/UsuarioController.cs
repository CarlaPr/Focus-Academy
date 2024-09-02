using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FocusAcademy.Repositorio;
using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Models;
using Microsoft.Extensions.Logging;

namespace FocusAcademy.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, ILogger<UsuarioController> logger)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _logger = logger;
        }

        public IActionResult Index() //login
        {
            return View();
        }

        public IActionResult Home() //login
        {
            return RedirectToAction("Index");
        }

        public IActionResult ListaUsuario()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(UsuarioModel usuario)
        {
            try
            {
                // Validação manual
                if (string.IsNullOrEmpty(usuario.Nome))
                {
                    ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
                }

                if (string.IsNullOrEmpty(usuario.Cpf))
                {
                    ModelState.AddModelError("Cpf", "O campo CPF é obrigatório.");
                }

                if (string.IsNullOrEmpty(usuario.Email) || !usuario.Email.Contains("@"))
                {
                    ModelState.AddModelError("Email", "O campo Email deve ser um endereço de email válido.");
                }

                // Continue com a validação para outros campos

                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.Cadastrar(usuario);
                    TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Se o modelo não é válido, retorna a mesma view com os erros
                    return View(usuario);
                }
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Houve um erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
