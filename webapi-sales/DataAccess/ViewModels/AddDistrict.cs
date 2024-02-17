using WebapiSales.DataAccess.Models;

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
        public int DistrictId { get; set; }
        public string SalesType { get; set; }

    }
    public class AddSalesPersonToDistrictViewModel{
        public int SalesPersonId { get; set; }
        public int DistrictId { get; set; }
        public string SalesType { get; set; } = "Secondary";

    }
    public class SalesPersonViewModel
    {
        
        public SalesPersonViewModel(DistrictViewModel district)
        {
            SalesPersonId = district.PrimarySalesId;
            DistrictId = district.DistrictId;
            FullName = district.PrimarySalesFullName;
            SalesType = "Primary";
        }
        public SalesPersonViewModel()
        {
            
        }
        public int SalesPersonId { get; set; }
        public int DistrictId { get; set; }
        public string FullName { get; set; }
        public  string SalesType { get; set; } = "Secondary";
    }

    public class AddSecondarySalesPerson
    {
        public int SalesPersonId { get; set; }
        public int DistrictId { get; set; }
    }

    public class DistrictViewModel
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int PrimarySalesId { get; set; }
        public string PrimarySalesFullName { get; set; }

    }
}
