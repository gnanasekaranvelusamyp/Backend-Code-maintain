using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using USERLOGIN_DEMO.Model;

namespace USERLOGIN_DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string connectionString = "Server=Y3_MAX\\SQLEXPRESS;Database=userlogin;Trusted_Connection=True;TrustServerCertificate=True;";


        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM Users WHERE Username = @username AND Password = @password";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", request.Username);
            cmd.Parameters.AddWithValue("@password", request.Password);

            SqlDataReader reader = cmd.ExecuteReader();

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
