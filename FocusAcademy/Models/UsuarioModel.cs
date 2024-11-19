using FocusAcademy.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FocusAcademy.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime? DataNascimento { get; set; }  // Adicionando DataNascimento

        public string Cpf { get; set; }

        public string Endereco { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public PerfilEnum Perfil { get; set; }
        public bool SenhaValida(string senha){
            return Senha == senha;
        }
        public List<MatriculaModel> Matriculas { get; set; } = new List<MatriculaModel>();
    }
}