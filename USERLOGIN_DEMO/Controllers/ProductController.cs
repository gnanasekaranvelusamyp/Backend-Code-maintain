using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using USERLOGIN_DEMO.Model;

namespace USERLOGIN_DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult SaveProduct(ProductMaster product)
        {
            string connectionString =
             _configuration.GetConnectionString("DefaultConnection");

            using NpgsqlConnection con =
                new NpgsqlConnection(connectionString);

            con.Open();

            string query = @"INSERT INTO Productmaster
                            (PartNo,PartName,UOM,HsnCode)
                             VALUES
                            (@PartNo,@PartName, @UOM, @HsnCode)";

            using NpgsqlCommand cmd =
                new NpgsqlCommand(query, con);

            cmd.Parameters.AddWithValue("@PartNo", product.Part_No);
            cmd.Parameters.AddWithValue("@PartName", product.Part_Name);
            cmd.Parameters.AddWithValue("@UOM", product.UOM);
            cmd.Parameters.AddWithValue("@HsnCode", product.Hsn_Code);

            cmd.ExecuteNonQuery();

            return Ok(new
            {
                message = "Saved Successfully"
            });
        }
    }
}
