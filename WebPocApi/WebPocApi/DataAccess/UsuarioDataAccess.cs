using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using WebPocApi.Models;

namespace WebPocApi.DataAccess
{
    public class UsuarioDataAccess
    {
        private IConfiguration _configuration;

        public UsuarioDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Usuario LocalizarUsuario(string usuarioID)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("DataBaseConnection")))
            {
                return conexao.QueryFirstOrDefault<Usuario>("SELECT UsuarioID, AccessKey " +
                    "FROM dbo.Usuario WHERE UsuarioID = @UsuarioID", new { UsuarioID = usuarioID });
            }
        }
    }
}
