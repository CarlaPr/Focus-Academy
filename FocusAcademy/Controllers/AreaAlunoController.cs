using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Models;
using FocusAcademy.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace FocusAcademy.Controllers
{
    public class AreaAlunoController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public AreaAlunoController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        // Método auxiliar para verificar se o usuário está logado
        private IActionResult VerificarLogin()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Erro"] = "Você precisa estar logado para acessar esta página.";
                return RedirectToAction("Index", "Login");
            }
            return null;
        }

        public IActionResult Index()
        {
            try
            {
                var redirectResult = VerificarLogin();
                if (redirectResult != null) return redirectResult;

                var userId = HttpContext.Session.GetInt32("UserId").Value;
                var usuario = _usuarioRepositorio.ObterUsuarioComMatriculas(userId);

                if (usuario == null)
                {
                    TempData["Erro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index", "Login");
                }

                ViewData["UsuarioNome"] = usuario.Nome;
                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro ao carregar a área do aluno. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Grade()
        {
            try
            {
                var redirectResult = VerificarLogin();
                if (redirectResult != null) return redirectResult;

                var userId = HttpContext.Session.GetInt32("UserId").Value;
                var aluno = _usuarioRepositorio.ObterUsuarioComMatriculas(userId);

                if (aluno == null || aluno.Matriculas == null || !aluno.Matriculas.Any())
                {
                    TempData["Erro"] = "Você não está matriculado em nenhum curso.";
                    return RedirectToAction("Index");
                }

                ViewData["UsuarioNome"] = aluno.Nome;
                return View(aluno);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro ao carregar a grade horária. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index");
            }
        }

        public IActionResult Visualizar()
        {
            try
            {
                var redirectResult = VerificarLogin();
                if (redirectResult != null) return redirectResult;

                var userId = HttpContext.Session.GetInt32("UserId").Value;
                var aluno = _usuarioRepositorio.ObterUsuarioPorId(userId);

                if (aluno == null)
                {
                    TempData["Erro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index", "Login");
                }

                ViewData["UsuarioNome"] = aluno.Nome;
                return View(aluno);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro ao carregar os dados do aluno. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Login");
            }
        }

        
        public IActionResult Editar()
        {
            try
            {
                var redirectResult = VerificarLogin();
                if (redirectResult != null) return redirectResult;

                var userId = HttpContext.Session.GetInt32("UserId").Value;
                var aluno = _usuarioRepositorio.ObterUsuarioPorId(userId);

                if (aluno == null)
                {
                    TempData["Erro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index", "Login");
                }

                ViewData["UsuarioNome"] = aluno.Nome;
                return View(aluno); // Retorna o formulário de edição com os dados do aluno
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro ao carregar os dados do aluno. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Editar(UsuarioModel usuario)
        {
            try
            {
                var redirectResult = VerificarLogin();
                if (redirectResult != null) return redirectResult;

                usuario.Id = HttpContext.Session.GetInt32("UserId").Value;
                _usuarioRepositorio.AtualizarUsuario(usuario);

                TempData["Sucesso"] = "Dados atualizados com sucesso!";
                return RedirectToAction("Visualizar");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro ao atualizar seus dados. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Visualizar");
            }
        }
        public IActionResult Matricular()
        {
            try
            {
                var redirectResult = VerificarLogin();
                if (redirectResult != null) return redirectResult;

                var userId = HttpContext.Session.GetInt32("UserId").Value;
                var usuario = _usuarioRepositorio.ObterUsuarioPorId(userId);
                ViewData["UsuarioNome"] = usuario.Nome;

                var cursosDisponiveis = _usuarioRepositorio.ObterCursosDisponiveis();
                return View(cursosDisponiveis);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro ao carregar a página de matrícula. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Matricular(int cursoId)
        {
            try
            {
                var redirectResult = VerificarLogin();
                if (redirectResult != null) return redirectResult;

                var usuarioId = HttpContext.Session.GetInt32("UserId").Value;
                var curso = _usuarioRepositorio.ObterCursosDisponiveis().FirstOrDefault(c => c.Id == cursoId);

                if (curso == null)
                {
                    TempData["Erro"] = "Curso inválido ou não disponível.";
                    return RedirectToAction("Matricular");
                }

                var usuario = _usuarioRepositorio.ObterUsuarioPorId(usuarioId);
                if (usuario.Matriculas != null && usuario.Matriculas.Any(m => m.CursoId == cursoId))
                {
                    TempData["Erro"] = "Você já está matriculado neste curso.";
                    return RedirectToAction("Index");
                }

                _usuarioRepositorio.MatricularCurso(usuarioId, cursoId);

                TempData["Sucesso"] = "Matrícula realizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Matricular");
            }
        }
    }
}
