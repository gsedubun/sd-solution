using Dapper;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;
using WebapiSales.Providers;

namespace WebapiSales.DataAccess.Repositories;

public class SecondarySalesPersonRepository : ISecondarySalesPersonRepository
{
    private readonly DbConnectionProvider _dbConnectionProvider;

    public SecondarySalesPersonRepository(DbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public IEnumerable<SecondarySalesPerson> GetSecondarySalesPersons()
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var secondarySalesPersons = connection.Query<SecondarySalesPerson>("SELECT SalesPersonId,DistrictId FROM SecondarySalesPerson").ToList();
        return secondarySalesPersons;
    }

    public IEnumerable<SalesPersonViewModel> GetSecondarySalesPersonByDistrictId(int districtId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var secondarySalesPerson = connection.Query<SalesPersonViewModel>(@"SELECT 
                sp.SalesPersonId
                ,DistrictId 
                ,s.FullName
                FROM SecondarySalesPerson sp 
                join SalesPerson s on sp.SalesPersonId=s.SalesPersonId WHERE  sp.DistrictId = @DistrictId", new {  DistrictId = districtId });
        return secondarySalesPerson;
    }

    public void AddSecondarySalesPerson(SecondarySalesPerson secondarySalesPerson)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("INSERT INTO SecondarySalesPerson (SalesPersonId,DistrictId) VALUES (@SalesPersonId,@DistrictId)", secondarySalesPerson);

    }

    public void UpdateSecondarySalesPerson(SecondarySalesPerson secondarySalesPerson)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("UPDATE SecondarySalesPerson SET SalesPersonId = @SalesPersonId, DistrictId = @DistrictId WHERE SalesPersonId = @SalesPersonId AND DistrictId = @DistrictId", secondarySalesPerson);

    }

    public void DeleteSecondarySalesPerson(int salesPersonId, int districtId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("DELETE FROM SecondarySalesPerson WHERE SalesPersonId = @SalesPersonId AND DistrictId = @DistrictId", new { SalesPersonId = salesPersonId, DistrictId = districtId });
    }

    public bool SecondarySalesPersonExists(int salesPersonId, int districtId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var secondarySalesPerson = connection.QueryFirstOrDefault<SecondarySalesPerson>("SELECT SalesPersonId,DistrictId FROM SecondarySalesPerson WHERE SalesPersonId = @SalesPersonId AND DistrictId = @DistrictId", 
            new { SalesPersonId = salesPersonId, DistrictId = districtId });
        return secondarySalesPerson != null;
    }
}