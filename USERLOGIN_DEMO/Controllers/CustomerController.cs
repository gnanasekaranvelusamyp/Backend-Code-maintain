using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        [HttpGet("GetCustomer")]

        public IActionResult GetCustomer()
        {

            string connectionString =
            _configuration.GetConnectionString("DefaultConnection");


            List<object> list = new List<object>();


            using NpgsqlConnection con =
            new NpgsqlConnection(connectionString);


            con.Open();


            string query = "select * from customers";

            NpgsqlCommand cmd = new NpgsqlCommand(query, con);

            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new Customer()
                {
                    customer_id = Convert.ToInt64(dr["customer_id"]),
                    customer_name = dr["customer_name"].ToString(),

                    mobile_no = dr["mobile_no"].ToString(),
                    email = dr["email"].ToString(),
                    address = dr["address"].ToString(),
                    city = dr["city"].ToString(),
                    state = dr["state"].ToString(),
                    pincode = dr["pincode"].ToString()
                });
            }


            con.Close();


            return Ok(list);

        }
    }
}