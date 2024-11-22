using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Models;
using FocusAcademy.Data;

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
            TempData.Remove("MensagemSucesso");
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(UsuarioModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var usuarioExistente = _usuarioRepositorio.ObterUsuarioPorEmail(model.Email) ?? 
                                _usuarioRepositorio.ObterUsuarioPorCpf(model.Cpf);

            if (usuarioExistente != null)
            {
                TempData["MensagemErro"] = "O email ou CPF já estão cadastrados.";
                return View();
            }
            
            _usuarioRepositorio.AdicionarUsuario(model);
            TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
            return RedirectToAction("Index", "Login");
        }
    }
}
