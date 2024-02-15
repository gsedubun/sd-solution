using webapi.DataAccess.Models;

namespace webapi.DataAccess.Interfaces;

public interface IStoreRepository
{
    IEnumerable<Store> GetStores();
    Store GetStore(int storeId);
    void AddStore(Store store);
    void UpdateStore(Store store);
    void DeleteStore(int storeId);
    bool StoreExists(int storeId);
}