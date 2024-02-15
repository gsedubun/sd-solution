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

    public IEnumerable<District> GetDistricts()
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var districts = connection.Query<District>("SELECT DistrictId,DistrictName,PrimarySalesId FROM District").ToList();
        return districts;
    }

    public District? GetDistrict(int districtId)
    {
        using var connection = _dbConnectionProvider.CreateConnection();
        var district = connection.QueryFirstOrDefault<District>("SELECT DistrictId,DistrictName,PrimarySalesId FROM District WHERE DistrictId = @DistrictId", new { DistrictId = districtId });
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
}