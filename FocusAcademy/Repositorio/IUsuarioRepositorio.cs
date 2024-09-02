using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FocusAcademy.Models;

namespace FocusAcademy.Repositorio
{
    public interface IUsuarioRepositorio
    {
        UsuarioModel BuscarPorID(int ID);
        UsuarioModel Adicionar(UsuarioModel usuario);
        UsuarioModel Atualizar(UsuarioModel usuario);
        bool Apagar (int id);
    }
}