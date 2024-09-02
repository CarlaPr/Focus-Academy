using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FocusAcademy.Models;
using FocusAcademy.Data;

namespace FocusAcademy.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _context;

        public UsuarioRepositorio(BancoContext bancoContext){
            this._context = bancoContext;
        }

        public UsuarioModel BuscarPorID( int id){
            return _context.Usuarios.FirstOrDefault(x => x.Id == id);
        }

        public List<UsuarioModel> BuscarPorTodos(){
            return _context.Usuarios.ToList();
        }

        public UsuarioModel Adicionar(UsuarioModel usuario){
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        public UsuarioModel Atualizar(UsuarioModel usuario){

            UsuarioModel usuarioDB = BuscarPorID(usuario.Id);

            if (usuarioDB == null) throw new Exception("Houve um erro na atualizaçao do cadastro");

            usuarioDB.Nome = usuario.Nome;
            usuarioDB.DataNascimento = usuario.DataNascimento;
            usuarioDB.Cpf = usuario.Cpf;
            usuarioDB.Endereco = usuario.Endereco;
            usuarioDB.Telefone = usuario.Telefone;
            usuarioDB.Email = usuario.Email;
            usuarioDB.Senha = usuario.Senha;
            usuarioDB.Perfil = usuario.Perfil;
            usuarioDB.Login = usuario.Login;

            _context.Usuarios.Update(usuarioDB);
            _context.SaveChanges();

            return usuarioDB;
        }

        public bool Apagar(int id){

            UsuarioModel usuarioDB = BuscarPorID(id);

            if (usuarioDB == null) throw new Exception("Houve um erro na exclusão do cadastro");

            _context.Usuarios.Remove(usuarioDB);
            _context.SaveChanges();

            return true;   
        }
    }

}