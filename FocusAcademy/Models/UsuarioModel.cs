using FocusAcademy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FocusAcademy.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public PerfilEnum Perfil { get; set; }
        public string Senha { get; set; }

    }
}