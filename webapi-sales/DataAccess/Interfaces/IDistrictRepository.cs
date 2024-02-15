using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;

namespace WebapiSales.DataAccess.Interfaces  
{
    public interface IDistrictRepository
    {
        IEnumerable<District> GetDistricts();
        District? GetDistrict(int districtId);
        void AddDistrict(District district);
        void UpdateDistrict(District district);
        void DeleteDistrict(int districtId);
        bool DistrictExists(int districtId);
    }
}
