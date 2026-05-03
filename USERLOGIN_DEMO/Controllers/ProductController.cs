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
        private readonly string connectionString =
              "Host=aws-1-ap-south-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.eyaoebwnpupwpeucamki;Password=Sekar@1996##;SSL Mode=Require;Trust Server Certificate=true;";

        [HttpPost]
        public IActionResult SaveProduct(ProductMaster product)
        {
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

            return Ok("Saved Successfully");
        }
    }
}
