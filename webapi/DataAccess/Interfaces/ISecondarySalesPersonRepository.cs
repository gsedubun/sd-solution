using webapi.DataAccess.Models;

namespace webapi.DataAccess.Interfaces;

public interface ISecondarySalesPersonRepository
{
    IEnumerable<SecondarySalesPerson> GetSecondarySalesPersons();
    IEnumerable<SecondarySalesPerson> GetSecondarySalesPersonByDistrictId(int districtId);
    void AddSecondarySalesPerson(SecondarySalesPerson secondarySalesPerson);
    void UpdateSecondarySalesPerson(SecondarySalesPerson secondarySalesPerson);
    void DeleteSecondarySalesPerson(int salesPersonId, int districtId);
    bool SecondarySalesPersonExists(int salesPersonId, int districtId);
}