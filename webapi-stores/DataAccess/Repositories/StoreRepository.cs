using Dapper;
using WebapiStores.DataAccess.Interfaces;
using WebapiStores.DataAccess.Models;
using WebapiStores.Providers;

namespace WebapiStores.DataAccess.Repositories;
public class StoreRepository : IStoreRepository
{
    private readonly DbConnectionProvider dbConnectionProvider;

    public StoreRepository(DbConnectionProvider dbConnectionProvider)
    {
        this.dbConnectionProvider = dbConnectionProvider;
    }

    public void AddStore(Store store)
    {
        using var connection =  dbConnectionProvider.CreateConnection();
        int newid = connection.ExecuteScalar<int>(@"INSERT INTO Store (StoreName,DistrictId,DistrictName) VALUES ( @StoreName, @DistrictId,@DistrictName); 
            SELECT SCOPE_IDENTITY();", store);
        store.StoreId = newid;
    }

    public void DeleteStore(int storeId)
    {
        using var connection =  dbConnectionProvider.CreateConnection();
        connection.Execute("DELETE FROM Store WHERE StoreId = @storeId", new { storeId });
    }

    public Store? GetStore(int storeId)
    {
        using var connection =  dbConnectionProvider.CreateConnection();
        return connection.QueryFirstOrDefault<Store>("SELECT StoreId,StoreName,DistrictId,DistrictName FROM Store WHERE StoreId = @storeId", new { storeId });
    }

    public IEnumerable<Store> GetStores()
    {
        using var connection =  dbConnectionProvider.CreateConnection();
        return connection.Query<Store>("SELECT StoreId,StoreName,DistrictId,DistrictName FROM Store").ToList();
    }

    public IEnumerable<Store> GetStoresForDistrict(int districtId)
    {
        using var connection =  dbConnectionProvider.CreateConnection();
        return connection.Query<Store>("SELECT StoreId,StoreName,DistrictId,DistrictName FROM Store WHERE DistrictId = @districtId", new { districtId }).ToList();
    }

    public bool StoreExists(int storeId)
    {
        using var connection =  dbConnectionProvider.CreateConnection();
        return connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM Store WHERE StoreId = @storeId", new { storeId });
    }

    public void UpdateStore(Store store)
    {
        using var connection =  dbConnectionProvider.CreateConnection();
        connection.Execute("UPDATE Store SET StoreName = @StoreName, DistrictId = @DistrictId,DistrictName=@DistrictName WHERE StoreId = @StoreId", store);
    }
}