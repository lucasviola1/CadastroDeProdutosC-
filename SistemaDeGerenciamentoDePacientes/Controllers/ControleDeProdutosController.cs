using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SistemaDeGerenciamentoDePacientes.Models;
using SistemaDeGerenciamentoDePacientes.Services;
using System;

namespace SistemaDeGerenciamentoDePacientes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControleDeProdutosController : ControllerBase
    {
        private readonly ILogger<ControleDeProdutosController> _logger;

        private readonly ILogin _login;

        private readonly string _connectionString;

        public ControleDeProdutosController(ILogger<ControleDeProdutosController> logger, IConfiguration configuration, ILogin login)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("LocalSqlServer");
            _login = login;
        }

        [HttpGet]
        [Route("/login")]

        public async Task<IActionResult> Login(string email, string senha)
        {
            string retorno = "";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                retorno = _login.LoginUsuario(email, senha, connection);
            }
            

            return new ObjectResult(new { obj = retorno });
        }

        [HttpPost]
        [Route("/login/cadastro")]

        public async Task<IActionResult> Cadastro(string email, string senha)
        {
            string retorno = "";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO Usuario (email, senha) VALUES (@email, @senha)", connection))
                {
                    command.Parameters.AddWithValue("@email", email.ToString());
                    command.Parameters.AddWithValue("@senha", senha.ToString());

                    command.ExecuteNonQuery();
                }
            }


            return Ok("Usuario cadastrado com sucesso!");
        }

        [HttpGet]
        [Route("/getprodutos")]

        public async Task<IActionResult> GetProdutos()
        {
            List<Produtos> listaRetorno = new List<Produtos>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT id, nome, preco, quantidade, descricao, quantidade FROM Produtos", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaRetorno.Add(new Produtos()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                nome = reader["nome"].ToString(),
                                preco = reader["preco"].ToString(),
                                descricao = reader["descricao"].ToString(),
                                quantidade = Convert.ToInt32(reader["quantidade"]),
                            });
                        }
                    }
                }
            }

            return Ok(new { obj = listaRetorno });
        }

        [HttpGet]
        [Route("/getprodutos/{nome}")]

        public async Task<IActionResult> GetProdutosNome(string nome)
        {
            List<Produtos> listaRetorno = new List<Produtos>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT id, nome, preco, quantidade, descricao, quantidade FROM Produtos WHERE nome LIKE '%{nome}%'", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaRetorno.Add(new Produtos()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                nome = reader["nome"].ToString(),
                                preco = reader["preco"].ToString(),
                                descricao = reader["descricao"].ToString(),
                                quantidade = Convert.ToInt32(reader["quantidade"]),
                            });
                        }
                    }
                }
            }

            return Ok(new { obj = listaRetorno });
        }

        [HttpPost]
        [Route("/addeditproduto")]

        public async Task<IActionResult> AddEditPaciente([FromBody] Produtos model)
        {
            if(model.id == 0 || model.id == null)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("INSERT INTO Produtos (nome, preco, quantidade, descricao) VALUES (@nome, @preco, @quantidade, @descricao)", connection))
                    {
                        command.Parameters.AddWithValue("nome", model.id);
                        command.Parameters.AddWithValue("preco", model.preco);
                        command.Parameters.AddWithValue("quantidade", model.quantidade);
                        command.Parameters.AddWithValue("descricao", model.descricao);

                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UPDATE Produtos SET nome = @nome, preco = @preco, quantidade = @quantidade, descricao = @descricao where id = @id;", connection))
                    {
                        command.Parameters.AddWithValue("id", model.id);
                        command.Parameters.AddWithValue("nome", model.nome);
                        command.Parameters.AddWithValue("preco", model.preco);
                        command.Parameters.AddWithValue("quantidade", model.quantidade);
                        command.Parameters.AddWithValue("descricao", model.descricao);

                        command.ExecuteNonQuery();
                    }
                }

            }

            

            return Ok("Ok!");
        }

        [HttpPost]
        [Route("/deleteproduto")]

        public async Task<IActionResult> DeletePaciente([FromQuery] int id)
        {
            List<Produtos> listaRetorno = new List<Produtos>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"DELETE FROM Produtos WHERE id = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return Ok(new { obj = listaRetorno });
        }
    }
}