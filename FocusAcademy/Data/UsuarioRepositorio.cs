using Microsoft.Data.SqlClient;
using FocusAcademy.Models;
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

                if (CpfExiste(usuario.Cpf))
                {
                    throw new ArgumentException("Este CPF já está cadastrado.");
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

        public bool CpfExiste(string cpf)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Usuarios WHERE Cpf = @Cpf", conn);
                cmd.Parameters.AddWithValue("@Cpf", cpf);

                return (int)cmd.ExecuteScalar() > 0;
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

                // Consulta principal para buscar os dados do usuário
                SqlCommand cmdUsuario = new SqlCommand("SELECT Id, Nome, Email, Perfil, DataNascimento, Cpf, Endereco, Telefone FROM Usuarios WHERE Id = @UserId", conn);
                cmdUsuario.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader readerUsuario = cmdUsuario.ExecuteReader())
                {
                    if (readerUsuario.Read())
                    {
                        usuario = new UsuarioModel
                        {
                            Id = (int)readerUsuario["Id"],
                            Nome = readerUsuario["Nome"].ToString(),
                            Email = readerUsuario["Email"].ToString(),
                            Perfil = (PerfilEnum)Enum.Parse(typeof(PerfilEnum), readerUsuario["Perfil"].ToString()),
                            DataNascimento = readerUsuario["DataNascimento"] as DateTime?,
                            Cpf = readerUsuario["Cpf"].ToString(),
                            Endereco = readerUsuario["Endereco"].ToString(),
                            Telefone = readerUsuario["Telefone"].ToString(),
                            Matriculas = new List<MatriculaModel>() // Inicializa a lista de matrículas
                        };
                    }
                }

                // Se o usuário foi encontrado, busca as matrículas associadas
                if (usuario != null)
                {
                    SqlCommand cmdMatriculas = new SqlCommand(@"
                        SELECT m.Id, m.CursoId, c.Nome AS CursoNome 
                        FROM Matriculas m
                        INNER JOIN Cursos c ON m.CursoId = c.Id
                        WHERE m.UsuarioId = @UserId", conn);
                    cmdMatriculas.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader readerMatriculas = cmdMatriculas.ExecuteReader())
                    {
                        while (readerMatriculas.Read())
                        {
                            var matricula = new MatriculaModel
                            {
                                Id = (int)readerMatriculas["Id"],
                                CursoId = (int)readerMatriculas["CursoId"],
                                Curso = new CursoModel
                                {
                                    Id = (int)readerMatriculas["CursoId"],
                                    Nome = readerMatriculas["CursoNome"].ToString()
                                }
                            };
                            usuario.Matriculas.Add(matricula);
                        }
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
                SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Nome = @Nome, Email = @Email, Cpf = @Cpf, DataNascimento = @DataNascimento, Endereco = @Endereco, Telefone = @Telefone WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                cmd.Parameters.AddWithValue("@DataNascimento", usuario.DataNascimento);
                cmd.Parameters.AddWithValue("@Endereco", usuario.Endereco);
                cmd.Parameters.AddWithValue("@Telefone", usuario.Telefone);
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
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Verificar se usuário existe
                        SqlCommand usuarioExistente = new SqlCommand("SELECT COUNT(1) FROM Usuarios WHERE Id = @UsuarioId", conn, transaction);
                        usuarioExistente.Parameters.AddWithValue("@UsuarioId", usuarioId);

                        // Verificar se curso existe
                        SqlCommand cursoExistente = new SqlCommand("SELECT COUNT(1) FROM Cursos WHERE Id = @CursoId", conn, transaction);
                        cursoExistente.Parameters.AddWithValue("@CursoId", cursoId);

                        if ((int)usuarioExistente.ExecuteScalar() == 0 || (int)cursoExistente.ExecuteScalar() == 0)
                        {
                            throw new ArgumentException("Usuário ou curso não encontrado.");
                        }

                        // Inserir matrícula
                        SqlCommand cmdMatricula = new SqlCommand("INSERT INTO Matriculas (UsuarioId, CursoId) VALUES (@UsuarioId, @CursoId)", conn, transaction);
                        cmdMatricula.Parameters.AddWithValue("@UsuarioId", usuarioId);
                        cmdMatricula.Parameters.AddWithValue("@CursoId", cursoId);

                        cmdMatricula.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void CancelarMatricula(int matriculaId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    Console.WriteLine($"Tentando cancelar matrícula com ID: {matriculaId}");

                    SqlCommand cmd = new SqlCommand("DELETE FROM Matriculas WHERE Id = @MatriculaId", conn);
                    cmd.Parameters.AddWithValue("@MatriculaId", matriculaId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    Console.WriteLine($"Linhas afetadas no banco de dados: {rowsAffected}");

                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("Matrícula não encontrada no banco de dados.");
                        throw new Exception("Matrícula não encontrada para cancelamento.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao cancelar matrícula: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
