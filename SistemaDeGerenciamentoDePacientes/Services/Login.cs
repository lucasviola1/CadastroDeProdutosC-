using Microsoft.Data.SqlClient;

namespace SistemaDeGerenciamentoDePacientes.Services
{
    public interface ILogin
    {
        string LoginUsuario(string? email, string? senha, SqlConnection connectionString);
    }

    public class Login : ILogin
    {
        public string LoginUsuario(string? email, string? senha, SqlConnection connectionString)
        {
            string result = "";
            try
            {
                using (SqlCommand command = new SqlCommand($"SELECT email FROM Usuario WHERE email = '{email}' and senha = '{senha}'", connectionString))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader["email"].ToString();

                            return result;
                        }
                        else
                        {
                            return "Usuario não encontrado";
                        }
                    }
                }
            } catch (Exception ex)
            {
                return "Erro" + ex.Message;
            
            }
        }
    }
}
