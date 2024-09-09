using System;
using FocusAcademy.Helper;
using FocusAcademy.Models;
using FocusAcademy.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace FocusAcademy.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private ISessao _sessao;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            //Se o usuario estiver logado ele sera redirecionado para a pagina area do aluno
            if(_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "AreaAluno");

            return View();
        }

        public IActionResult Sair(){
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
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
                            _sessao.CriarSessaoDoUsuario(usuario);

                            if (usuario.Perfil == FocusAcademy.Enums.PerfilEnum.Admin){
                                return RedirectToAction("IndexAdmin", "AreaAluno");
                            }else{
                                return RedirectToAction("Index", "AreaAluno");
                            }                
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
