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
        public int? SalesPersonId { get; set; }

        public string FullName { get; set; }

    }
    /// <summary>
    /// Add SalesPerson to District
    /// </summary>
    public class AddSalesPersonToDistrictViewModel{
        /// <summary>
        /// SalesPerson Id
        /// </summary>
        public int SalesPersonId { get; set; }
        /// <summary>
        /// District Id
        /// </summary>
        public int DistrictId { get; set; }
        /// <summary>
        /// SalesType : Primary or Secondary
        /// </summary>
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
        /// <summary>
        /// SalesPerson Id
        /// </summary>
        public int SalesPersonId { get; set; }
        /// <summary>
        /// District Id
        /// </summary>
        public int DistrictId { get; set; }
        /// <summary>
        /// SalesPerson FullName
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// SalesType : Primary or Secondary
        /// </summary>
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
