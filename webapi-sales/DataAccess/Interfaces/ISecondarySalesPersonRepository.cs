using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;

namespace WebapiSales.DataAccess.Interfaces;

public interface ISecondarySalesPersonRepository
{
    IEnumerable<SecondarySalesPerson> GetSecondarySalesPersons();
    IEnumerable<SalesPersonViewModel> GetSecondarySalesPersonByDistrictId(int districtId);
    void AddSecondarySalesPerson(SecondarySalesPerson secondarySalesPerson);
    void UpdateSecondarySalesPerson(SecondarySalesPerson secondarySalesPerson);
    void DeleteSecondarySalesPerson(int salesPersonId, int districtId);
    bool SecondarySalesPersonExists(int salesPersonId, int districtId);
}