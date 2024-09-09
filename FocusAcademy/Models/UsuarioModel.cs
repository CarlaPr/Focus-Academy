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
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [StringLength(11)]
        public string Cpf { get; set; }

        [StringLength(200)]
        public string Endereco { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Senha { get; set; }

        [Required]
        public PerfilEnum Perfil { get; set; }
        public bool SenhaValida(string senha){
            return Senha == senha;
        }
    }
}