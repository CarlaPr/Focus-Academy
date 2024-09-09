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

    public UsuarioRepositorio(BancoContext context)
    {
        _context = context;
    }

    public UsuarioModel BuscarPorID(int id)
    {
        return _context.Usuarios.FirstOrDefault(x => x.Id == id);
    }

    public List<UsuarioModel> BuscarTodos()
    {
        return _context.Usuarios.ToList();
    }

    public UsuarioModel Cadastrar(UsuarioModel usuario)
    {
        try
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }
        catch (Exception ex)
        {
            // Logar a exceção
            Console.WriteLine($"Erro ao cadastrar usuário: {ex.Message}");
            throw;
        }
    }

    public UsuarioModel Atualizar(UsuarioModel usuario)
    {
        var usuarioDB = BuscarPorID(usuario.Id);
        if (usuarioDB == null) throw new Exception("Houve um erro na atualização do cadastro");

        usuarioDB.Nome = usuario.Nome;
        usuarioDB.DataNascimento = usuario.DataNascimento;
        usuarioDB.Cpf = usuario.Cpf;
        usuarioDB.Endereco = usuario.Endereco;
        usuarioDB.Telefone = usuario.Telefone;
        usuarioDB.Email = usuario.Email;
        usuarioDB.Senha = usuario.Senha;
        usuarioDB.Perfil = usuario.Perfil;

        _context.Usuarios.Update(usuarioDB);
        _context.SaveChanges();
        return usuarioDB;
    }

    public bool Apagar(int id)
    {
        var usuarioDB = BuscarPorID(id);
        if (usuarioDB == null) throw new Exception("Houve um erro na exclusão do cadastro");

        _context.Usuarios.Remove(usuarioDB);
        _context.SaveChanges();

        return true;
    }

        public UsuarioModel BuscarPorEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToLower());
        }
    }

}