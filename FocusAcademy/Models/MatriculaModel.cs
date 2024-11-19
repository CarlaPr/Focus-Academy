using System;

namespace FocusAcademy.Models
{
    public class MatriculaModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }
        public int CursoId { get; set; }
        public CursoModel Curso { get; set; }
        public IEnumerable<CursoModel> CursosDisponiveis { get; set; }

    }
}
