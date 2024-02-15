namespace webapi.DataAccess.Models
{
    public class SalesPerson
    {
        public int SalesPersonId { get; set; }
        public string FullName { get; set; }

    }

    public  class District  {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int PrimarySalesId{ get; set; }

    }

    public class SecondarySalesPerson
    {
        public int SalesPersonId { get; set; }
        public int DistrictId { get; set; }
    }

    public class Store   {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int DistrictId { get; set; }
    }
}
