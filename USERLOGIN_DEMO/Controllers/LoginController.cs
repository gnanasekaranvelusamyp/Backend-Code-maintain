using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql; // ✅ change
using USERLOGIN_DEMO.Model;

namespace USERLOGIN_DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string connectionString =
             _configuration.GetConnectionString("DefaultConnection");

            using NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM users WHERE LOWER(username) = LOWER(@username) AND password = @password";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", request.Username ?? "");
            cmd.Parameters.AddWithValue("@password", request.Password ?? "");

            NpgsqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                return Ok(new { message = "Login successful" });
            }
            else
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }
    }
}