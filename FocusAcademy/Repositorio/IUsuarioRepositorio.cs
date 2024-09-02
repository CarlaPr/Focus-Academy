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
        UsuarioModel Cadastrar(UsuarioModel usuario);
        List<UsuarioModel> BuscarTodos();
        UsuarioModel Atualizar(UsuarioModel usuario);
        bool Apagar (int id);
    }
}