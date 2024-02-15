using webapi.DataAccess.Models;
using webapi.DataAccess.ViewModels;

namespace webapi.DataAccess.Interfaces  
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
