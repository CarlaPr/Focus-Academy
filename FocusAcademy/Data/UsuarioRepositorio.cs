using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FocusAcademy.Models;
using Microsoft.Extensions.Configuration;
using FocusAcademy.Enums;

namespace FocusAcademy.Data
{
    public class UsuarioRepositorio
    {
        private readonly string _connectionString;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DataBase");

        }
        public void AdicionarUsuario(UsuarioModel usuario)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                usuario.Cpf = new string(usuario.Cpf.Where(char.IsDigit).ToArray());

                if (usuario.Cpf.Length != 11)
                {
                    throw new ArgumentException("O CPF deve conter exatamente 11 dígitos.");
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO Usuarios (Nome, Email, Senha, Cpf, DataNascimento, Endereco, Telefone, Perfil) VALUES (@Nome, @Email, @Senha, @Cpf, @DataNascimento, @Endereco, @Telefone, @Perfil)", conn);
                
                cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Senha", usuario.Senha);
                cmd.Parameters.AddWithValue("@Cpf", usuario.Cpf); // CPF limpo
                cmd.Parameters.AddWithValue("@DataNascimento", usuario.DataNascimento);
                cmd.Parameters.AddWithValue("@Endereco", usuario.Endereco);
                cmd.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                
                // Atribui perfil padrão caso seja zero
                if (usuario.Perfil == 0)
                {
                    usuario.Perfil = PerfilEnum.Padrao;
                }
                
                // Passa o valor numérico do enum para o banco de dados
                cmd.Parameters.AddWithValue("@Perfil", (int)usuario.Perfil);

                cmd.ExecuteNonQuery();
            }
        }

        public UsuarioModel ValidarLogin(string email, string senha)
        {
            UsuarioModel usuario = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Email = @Email AND Senha = @Senha", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senha);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new UsuarioModel
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            Perfil = (PerfilEnum)Enum.Parse(typeof(PerfilEnum), reader["Perfil"].ToString()) // Conversão do valor do banco para enum
                        };
                    }
                }
            }

            return usuario;
        }

        public UsuarioModel ObterUsuarioComMatriculas(int userId)
        {
            UsuarioModel usuario = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                string query = @"
                    SELECT u.Id, u.Nome, u.Email, u.Perfil, 
                        m.Id AS MatriculaId, c.Nome AS CursoNome 
                    FROM Usuarios u
                    LEFT JOIN Matriculas m ON u.Id = m.UsuarioId
                    LEFT JOIN Cursos c ON m.CursoId = c.Id
                    WHERE u.Id = @UserId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (usuario == null)
                        {
                            usuario = new UsuarioModel
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Perfil = (PerfilEnum)Enum.Parse(typeof(PerfilEnum), reader["Perfil"].ToString()),
                                Matriculas = new List<MatriculaModel>()
                            };
                        }

                        if (reader["MatriculaId"] != DBNull.Value)
                        {
                            MatriculaModel matricula = new MatriculaModel
                            {
                                Id = (int)reader["MatriculaId"],
                                Curso = new CursoModel
                                {
                                    Nome = reader["CursoNome"].ToString()
                                }
                            };

                            usuario.Matriculas.Add(matricula);
                        }
                        else
                        {
                            Console.WriteLine("Nenhuma matrícula encontrada para este usuário.");
                        }
                    }
                }
            }
        return usuario;
        }
        public List<CursoModel> ObterCursosDisponiveis()
        {
            List<CursoModel> cursos = new List<CursoModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Cursos", conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cursos.Add(new CursoModel
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Descricao = reader["Descricao"].ToString(),
                                MaxAlunos = (int)reader["MaxAlunos"]
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao obter cursos: {ex.Message}");
            }

            return cursos;
        }
        public UsuarioModel ObterUsuarioPorId(int userId)
        {
            UsuarioModel usuario = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, Nome, Email, Perfil, DataNascimento, Cpf, Endereco, Telefone FROM Usuarios WHERE Id = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new UsuarioModel
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            Perfil = (PerfilEnum)Enum.Parse(typeof(PerfilEnum), reader["Perfil"].ToString()),
                            DataNascimento = reader["DataNascimento"] as DateTime?,  // Adicionando DataNascimento
                            Cpf = reader["Cpf"].ToString(),  // Adicionando Cpf
                            Endereco = reader["Endereco"].ToString(),  // Adicionando Endereco
                            Telefone = reader["Telefone"].ToString()  // Adicionando Telefone
                        };
                    }
                }
            }

            return usuario;
        }

       public void AtualizarUsuario(UsuarioModel usuario)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Verifique se o Cpf não é nulo ou vazio antes de realizar a atualização
                if (string.IsNullOrEmpty(usuario.Cpf))
                {
                    throw new ArgumentException("O CPF não pode ser nulo ou vazio.", nameof(usuario.Cpf));
                }

                // Log para depuração
                Console.WriteLine($"Atualizando usuário {usuario.Id} com CPF: {usuario.Cpf}");

                // Atualiza os dados do usuário
                SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Nome = @Nome, Email = @Email, Cpf = @Cpf, DataNascimento = @DataNascimento, Endereco = @Endereco, Telefone = @Telefone, Senha = @Senha WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Cpf", usuario.Cpf);  // Certifique-se de que @Cpf está sendo passado corretamente
                cmd.Parameters.AddWithValue("@DataNascimento", usuario.DataNascimento);
                cmd.Parameters.AddWithValue("@Endereco", usuario.Endereco);
                cmd.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                cmd.Parameters.AddWithValue("@Senha", usuario.Senha);
                cmd.Parameters.AddWithValue("@Id", usuario.Id);

                cmd.ExecuteNonQuery();
            }
        }
        public UsuarioModel ObterUsuarioPorCpf(string cpf)
        {
            UsuarioModel usuario = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Cpf = @Cpf", conn);
                cmd.Parameters.AddWithValue("@Cpf", cpf);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new UsuarioModel
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            Perfil = (PerfilEnum)Enum.Parse(typeof(PerfilEnum), reader["Perfil"].ToString())
                        };
                    }
                }
            }

            return usuario;
        }

        public UsuarioModel ObterUsuarioPorEmail(string email)
        {
            UsuarioModel usuario = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new UsuarioModel
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            Perfil = (PerfilEnum)Enum.Parse(typeof(PerfilEnum), reader["Perfil"].ToString())
                        };
                    }
                }
            }

            return usuario;
        }

        public void MatricularCurso(int usuarioId, int cursoId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Verificar se o usuário e o curso existem
                var usuarioExistente = new SqlCommand("SELECT COUNT(1) FROM Usuarios WHERE Id = @UsuarioId", conn);
                usuarioExistente.Parameters.AddWithValue("@UsuarioId", usuarioId);

                var cursoExistente = new SqlCommand("SELECT COUNT(1) FROM Cursos WHERE Id = @CursoId", conn);
                cursoExistente.Parameters.AddWithValue("@CursoId", cursoId);

                if ((int)usuarioExistente.ExecuteScalar() == 0 || (int)cursoExistente.ExecuteScalar() == 0)
                {
                    throw new ArgumentException("Usuário ou curso não encontrado.");
                }

                // Inserir a matrícula
                SqlCommand cmdMatricula = new SqlCommand("INSERT INTO Matriculas (UsuarioId, CursoId) VALUES (@UsuarioId, @CursoId)", conn);
                cmdMatricula.Parameters.AddWithValue("@UsuarioId", usuarioId);
                cmdMatricula.Parameters.AddWithValue("@CursoId", cursoId);

                cmdMatricula.ExecuteNonQuery();
            }
        }
    }
}
