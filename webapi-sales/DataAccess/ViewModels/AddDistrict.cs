namespace WebapiSales.DataAccess.ViewModels
{
    public class AddDistrict
    {
            public string DistrictName { get; set; }

            public int PrimarySalesId { get; set; }

    }

    public class UpdateDistrict
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int PrimarySalesId { get; set; }

    }

    public class AddSalesPerson
    {
        public string FullName { get; set; }

    }

}
