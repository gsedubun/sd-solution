using Dapper;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;
using WebapiSales.Providers;

namespace WebapiSales.DataAccess.Repositories;

public class DistrictRepository : IDistrictRepository
{
    private readonly DbConnectionProvider _dbConnectionProvider;

    public DistrictRepository(DbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public IEnumerable<DistrictViewModel> GetDistricts()
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var districts = connection.Query<DistrictViewModel>(@"SELECT DistrictId,DistrictName,PrimarySalesId,sp.FullName PrimarySalesFullName  
                    FROM District d join SalesPerson sp on d.PrimarySalesId=sp.SalesPersonId ").ToList();
        return districts;
    }

    public DistrictViewModel? GetDistrict(int districtId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var district = connection.QueryFirstOrDefault<DistrictViewModel>("SELECT DistrictId,DistrictName,PrimarySalesId,sp.FullName PrimarySalesFullName  " +
                                                                         " FROM District d join SalesPerson sp on d.PrimarySalesId=sp.SalesPersonId WHERE DistrictId = @DistrictId", new { DistrictId = districtId });
        return district;
    }

    public void AddDistrict(District district)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        int newid= connection.ExecuteScalar<int>(@"INSERT INTO District(DistrictName,PrimarySalesId) 
                OUTPUT INSERTED.DistrictId
                VALUES (@DistrictName,@PrimarySalesId)", param: district);
        district.DistrictId = newid;
    }

    public void UpdateDistrict(District district)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("UPDATE District SET DistrictName = @DistrictName, PrimarySalesId = @PrimarySalesId WHERE DistrictId = @DistrictId", district);
    }

    public void DeleteDistrict(int districtId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        connection.Execute("DELETE FROM District WHERE DistrictId = @DistrictId", new { DistrictId = districtId });
    }

    public bool DistrictExists(int districtId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var district = connection.QueryFirstOrDefault<District>("SELECT DistrictId FROM District WHERE DistrictId = @DistrictId", new { DistrictId = districtId });
        return district != null;
    }

    public IEnumerable<SalesPerson> GetAvailableSalesPersonsForDistrict(int districtId)
    {
        string sql = @"select s.SalesPersonId, s.FullName, sp.DistrictId , d.DistrictName
                from SalesPerson s
                left join SecondarySalesPerson sp on s.SalesPersonId=sp.SalesPersonId and  sp.DistrictId=@districtid
                left join District d on s.SalesPersonId=d.PrimarySalesId and d.DistrictId=@districtid
                where sp.DistrictId is null and d.DistrictId is null";
        using var connection = _dbConnectionProvider.CreateConnection();
        var salesPersons = connection.Query<SalesPerson>(sql, new { districtid = districtId }).ToList();
        return salesPersons;
    }
}