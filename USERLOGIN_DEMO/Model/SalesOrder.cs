namespace USERLOGIN_DEMO.Model
{
    public class SalesOrder
    {
        public string Customer { get; set; }

        public string OrderNo { get; set; }

        public List<SalesOrderItem> Items { get; set; }
    }
}
