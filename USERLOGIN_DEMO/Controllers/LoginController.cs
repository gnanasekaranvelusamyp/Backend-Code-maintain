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
        // ✅ Supabase connection string
        private readonly string connectionString =
              "Host=aws-1-ap-south-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.eyaoebwnpupwpeucamki;Password=Sekar@1996##;SSL Mode=Require;Trust Server Certificate=true;";
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
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