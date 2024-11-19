using System;
using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Models;
using FocusAcademy.Data;
using FocusAcademy.Enums;

namespace FocusAcademy.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        // Processa o cadastro
        [HttpPost]
        public IActionResult Cadastrar(UsuarioModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Cadastro");
            }

            // Verifica se o CPF ou o email já estão cadastrados
            var usuarioExistentePorEmail = _usuarioRepositorio.ObterUsuarioPorEmail(model.Email);
            var usuarioExistentePorCpf = _usuarioRepositorio.ObterUsuarioPorCpf(model.Cpf);

            if (usuarioExistentePorEmail != null || usuarioExistentePorCpf != null)
            {
                TempData["MensagemErro"] = "O email ou CPF já estão cadastrados. Tente novamente com outros dados.";
                return View("Cadastro");
            }

            // Adiciona o novo usuário ao banco de dados
            _usuarioRepositorio.AdicionarUsuario(model);

            TempData["MensagemSucesso"] = "Cadastro realizado com sucesso. Você já pode fazer login!";
            return RedirectToAction("Index", "Login");
        }
    }
}

