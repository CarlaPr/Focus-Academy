using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Models;
using FocusAcademy.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace FocusAcademy.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public MatriculaController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        // Exibe a página de cursos disponíveis para matrícula
        public IActionResult Matricular()
        {
            var cursos = _usuarioRepositorio.ObterCursosDisponiveis();
            return View(cursos); // Passa a lista de cursos para a view
        }

        [HttpPost]
public IActionResult Matricular(int cursoId)
{
    try
    {
        var usuarioId = HttpContext.Session.GetInt32("UserId");
        if (usuarioId == null)
        {
            TempData["Erro"] = "Você precisa estar logado para se matricular.";
            return RedirectToAction("Index", "Login");
        }

        var usuario = _usuarioRepositorio.ObterUsuarioPorId(usuarioId.Value);
        if (usuario == null)
        {
            TempData["Erro"] = "Usuário não encontrado.";
            return RedirectToAction("Index", "Login");
        }

        if (usuario.DataNascimento < new DateTime(1753, 1, 1))
        {
            TempData["Erro"] = "A data de nascimento está fora do intervalo permitido.";
            return RedirectToAction("Matricular");
        }

        var curso = _usuarioRepositorio.ObterCursosDisponiveis().FirstOrDefault(c => c.Id == cursoId);
        if (curso == null)
        {
            TempData["Erro"] = "Curso inválido ou não disponível.";
            return RedirectToAction("Matricular");
        }

        if (usuario.Matriculas != null && usuario.Matriculas.Any(m => m.CursoId == cursoId))
        {
            TempData["Erro"] = "Você já está matriculado neste curso.";
            return RedirectToAction("Index");
        }

        usuario.Matriculas ??= new List<MatriculaModel>();

        usuario.Matriculas.Add(new MatriculaModel
        {
            UsuarioId = usuarioId.Value,
            CursoId = cursoId,
        });

        _usuarioRepositorio.AtualizarUsuario(usuario);

        TempData["Sucesso"] = "Matrícula realizada com sucesso!";
        return RedirectToAction("Index");
    }
    catch (SqlException sqlEx)
    {
        TempData["Erro"] = $"Erro ao tentar realizar a matrícula. Detalhes: {sqlEx.Message}";
        Console.WriteLine(sqlEx.Message);
    }
    catch (Exception ex)
    {
        TempData["Erro"] = $"Ocorreu um erro inesperado. Detalhes: {ex.Message}";
        Console.WriteLine(ex.Message);
    }
    return RedirectToAction("Matricular");
}


    }
}
