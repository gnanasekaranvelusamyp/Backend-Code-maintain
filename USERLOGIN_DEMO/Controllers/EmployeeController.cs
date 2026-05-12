using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Npgsql.Replication.PgOutput.Messages;
using USERLOGIN_DEMO.Model;

namespace USERLOGIN_DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee()
        {
            List<Employee> list = new List<Employee>();

            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            NpgsqlConnection con = new NpgsqlConnection(connectionString);

            con.Open();

            string query = "SELECT * FROM employee";

            NpgsqlCommand cmd = new NpgsqlCommand(query, con);

            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new Employee()
                {
                    id = Convert.ToInt32(dr["id"]),
                    name = dr["Employeename"].ToString(),
                    payment = Convert.ToDecimal(dr["salary"])
                });
            }

            con.Close();

            return Ok(list);
        }

        [HttpPost("Post")]
        public IActionResult Employeefill(Employee set)
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            NpgsqlConnection con =
                new NpgsqlConnection(connectionString);

            con.Open();

            string query =
                "insert into employee(employeename,salary) values(@name,@salary)";

            NpgsqlCommand cmd =
                new NpgsqlCommand(query, con);

            cmd.Parameters.AddWithValue("@name", set.name);

            cmd.Parameters.AddWithValue("@salary", set.payment);

            cmd.ExecuteNonQuery();

            con.Close();

            return Ok();
        }
    }
}
