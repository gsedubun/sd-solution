using WebapiSales.DataAccess.Models;

namespace WebapiSales.DataAccess.Interfaces;

public interface ISalesPersonRepository
{
    IEnumerable<SalesPerson> GetSalesPersons();
    SalesPerson? GetSalesPerson(int salesPersonId);
    void AddSalesPerson(SalesPerson salesPerson);
    void UpdateSalesPerson(SalesPerson salesPerson);
    void DeleteSalesPerson(int salesPersonId);
    bool SalesPersonExists(int salesPersonId);
}