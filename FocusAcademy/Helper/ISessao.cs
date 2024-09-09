using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FocusAcademy.Models;

namespace FocusAcademy.Helper
{
    public interface ISessao
    {
        void CriarSessaoDoUsuario(UsuarioModel usuario);
        void RemoverSessaoDoUsuario();
        UsuarioModel BuscarSessaoDoUsuario();
    }
}