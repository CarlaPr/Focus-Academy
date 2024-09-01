using FocusAcademy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace FocusAcademy.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        //configuração da usuario model que representa a tabela, e a tabela usuarios
        public DbSet<UsuarioModel> Usuarios { get; set; }

    }
}