using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using USERLOGIN_DEMO.Model;

namespace USERLOGIN_DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SalesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("SaveOrder")]
        public IActionResult SaveOrder(
    [FromBody] SalesOrder order)
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            using NpgsqlConnection con =
                new NpgsqlConnection(connectionString);

            con.Open();

            string headerQuery =
            @"insert into salesorderheader
              (orderno, customer)

              values
               (@orderno, @customer)

              returning id";

            NpgsqlCommand headerCmd =
                new NpgsqlCommand(headerQuery, con);

            headerCmd.Parameters.AddWithValue(
                "@orderno", order.OrderNo);

            headerCmd.Parameters.AddWithValue(
                "@customer", order.Customer);

            int headerId =
                Convert.ToInt32(
                    headerCmd.ExecuteScalar());

            foreach (var item in order.Items)
            {
                string detailQuery =
                @"insert into salesorderdetails
          (headerid, partno,
           partname, price,
           qty, amount)

          values
          (@headerid, @partno,
           @partname, @price,
           @qty, @amount)";

                NpgsqlCommand detailCmd =
                    new NpgsqlCommand(detailQuery, con);

                detailCmd.Parameters.AddWithValue(
                    "@headerid", headerId);

                detailCmd.Parameters.AddWithValue(
                    "@partno", item.PartNo);

                detailCmd.Parameters.AddWithValue(
                    "@partname", item.PartName);

                detailCmd.Parameters.AddWithValue(
                    "@price", item.Price);

                detailCmd.Parameters.AddWithValue(
                    "@qty", item.Qty);

                detailCmd.Parameters.AddWithValue(
                    "@amount", item.Amount);

                detailCmd.ExecuteNonQuery();
            }

            return Ok("Saved");
        }
    }
}
