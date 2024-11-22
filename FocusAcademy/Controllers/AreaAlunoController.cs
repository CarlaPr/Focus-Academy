using Microsoft.AspNetCore.Mvc;
using FocusAcademy.Models;
using FocusAcademy.Data;

namespace FocusAcademy.Controllers
{
    public class AreaAlunoController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public AreaAlunoController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        private IActionResult VerificarLogin()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["MensagemErro"] = "Você precisa estar logado para acessar a Área do Aluno";
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
                    TempData["MensagemErro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index", "Login");
                }

                ViewData["UsuarioNome"] = usuario.Nome;
                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao carregar a área do aluno. Tente novamente mais tarde.";
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
                    TempData["ErroMatricular"] = "Você não está matriculado em nenhum curso.";
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

                TempData["SucessoEditar"] = "Dados atualizados com sucesso!";
                return RedirectToAction("Visualizar");
            }
            catch (Exception ex)
            {
                TempData["ErroEditar"] = "Ocorreu um erro ao atualizar seus dados. Tente novamente mais tarde.";
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
                TempData["ErroMatricular"] = "Ocorreu um erro ao carregar a página de matrícula. Tente novamente mais tarde.";
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
                    TempData["ErroMatricular"] = "Curso inválido ou não disponível.";
                    return RedirectToAction("Index");
                }

                var usuario = _usuarioRepositorio.ObterUsuarioPorId(usuarioId);
                if (usuario.Matriculas != null && usuario.Matriculas.Any(m => m.CursoId == cursoId))
                {
                    TempData["ErroMatricular"] = "Você já está matriculado neste curso.";
                    return RedirectToAction("Index");
                }

                _usuarioRepositorio.MatricularCurso(usuarioId, cursoId);

                TempData["SucessoMatricular"] = "Matrícula realizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErroMatricular"] = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("Matricular");
            }
        }

       public IActionResult ConfirmarCancelamento(int userId, int matriculaId)
        {
            try
            {
                Console.WriteLine($"Ação ConfirmarCancelamento chamada para MatrículaId: {matriculaId} e UserId: {userId}");
                
                var usuario = _usuarioRepositorio.ObterUsuarioComMatriculas(userId);

                if (usuario == null)
                {
                    TempData["ErroCancelar"] = "Usuário não encontrado.";
                    return RedirectToAction("Index");
                }

                var matricula = usuario.Matriculas.FirstOrDefault(m => m.Id == matriculaId);

                if (matricula == null)
                {
                    TempData["ErroCancelar"] = "Matrícula não encontrada.";
                    return RedirectToAction("Index");
                }

                ViewData["UserId"] = userId;
                ViewData["MatriculaId"] = matriculaId;
                ViewData["UsuarioNome"] = usuario.Nome;

                return View(matricula);
            }
            catch (Exception ex)
            {
                TempData["ErroCancelar"] = $"Erro ao carregar a confirmação de cancelamento: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ConfirmarCancelamentoPost(int userId, int matriculaId)
        {
            try
            {
                Console.WriteLine($"ConfirmarCancelamentoPost chamado para UserId: {userId} e MatriculaId: {matriculaId}");

                if (userId == 0 || matriculaId == 0)
                {
                    TempData["ErroCancelar"] = "Parâmetros inválidos para cancelamento.";
                    return RedirectToAction("Index", "AreaAluno");
                }

                var usuario = _usuarioRepositorio.ObterUsuarioComMatriculas(userId);
                if (usuario == null)
                {
                    TempData["ErroCancelar"] = "Usuário não encontrado.";
                    return RedirectToAction("Index", "AreaAluno");
                }

                var matricula = usuario.Matriculas.FirstOrDefault(m => m.Id == matriculaId);
                if (matricula == null)
                {
                    TempData["ErroCancelar"] = "Matrícula não encontrada.";
                    return RedirectToAction("Index", "AreaAluno");
                }

                // Cancelar a matrícula
                _usuarioRepositorio.CancelarMatricula(matriculaId);
                TempData["SucessoCancelar"] = "Matrícula cancelada com sucesso.";

                return RedirectToAction("Index", "AreaAluno");
            }
            catch (Exception ex)
            {
                TempData["ErroCancelar"] = $"Erro ao cancelar matrícula: {ex.Message}";
                Console.WriteLine($"Erro ao cancelar matrícula: {ex.Message}");
                return RedirectToAction("Index", "AreaAluno");
            }
        }
    }
}
