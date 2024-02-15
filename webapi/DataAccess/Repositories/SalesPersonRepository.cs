using Dapper;
using webapi.DataAccess.Interfaces;
using webapi.DataAccess.Models;
using webapi.Providers;

namespace webapi.DataAccess.Repositories;

public class SalesPersonRepository : ISalesPersonRepository
{
    private readonly DbConnectionProvider _dbConnectionProvider;

    public SalesPersonRepository(DbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public IEnumerable<SalesPerson> GetSalesPersons()
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var salesPersons = connection.Query<SalesPerson>("SELECT SalesPersonId,FullName FROM SalesPerson").ToList();
        return salesPersons;
    }

    public SalesPerson? GetSalesPerson(int salesPersonId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var salesPerson = connection.QueryFirstOrDefault<SalesPerson>("SELECT SalesPersonId,FullName FROM SalesPerson WHERE SalesPersonId = @SalesPersonId", new { SalesPersonId = salesPersonId });
        return salesPerson;
    }

    public void AddSalesPerson(SalesPerson salesPerson)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("INSERT INTO SalesPerson (FullName) VALUES (@FullName)", salesPerson);

    }

    public void UpdateSalesPerson(SalesPerson salesPerson)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("UPDATE SalesPerson SET FullName = @FullName WHERE SalesPersonId = @SalesPersonId", salesPerson);

    }

    public void DeleteSalesPerson(int salesPersonId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("DELETE FROM SalesPerson WHERE SalesPersonId = @SalesPersonId", new { SalesPersonId = salesPersonId });

    }

    public bool SalesPersonExists(int salesPersonId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var salesPerson = connection.QueryFirstOrDefault<SalesPerson>("SELECT SalesPersonId FROM SalesPerson WHERE SalesPersonId = @SalesPersonId", new { SalesPersonId = salesPersonId });
        return salesPerson != null;
    }

   
}