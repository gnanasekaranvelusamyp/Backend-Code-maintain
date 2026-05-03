using Microsoft.AspNetCore.Mvc;
using Npgsql;
using USERLOGIN_DEMO.Model;

namespace USERLOGIN_DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            string connectionString =
             _configuration.GetConnectionString("DefaultConnection");

            using NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = @"INSERT INTO customers
            (customer_name, mobile_no, email, address, city, state, pincode)
            VALUES
            (@customer_name,@mobile_no, @email,@address, @city, @state, @pincode)";

            using NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@customer_name", customer.customer_name);
            cmd.Parameters.AddWithValue("@mobile_no", (object?)customer.mobile_no ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)customer.email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)customer.address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@city", (object?)customer.city ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@state", (object?)customer.state ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@pincode", (object?)customer.pincode ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            return Ok(new
            {
                message = "Saved Successfully"
            });
        }
    }
}