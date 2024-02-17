using Microsoft.AspNetCore.Mvc;
using WebapiStores.DataAccess.Interfaces;
using WebapiStores.DataAccess.Models;
using WebapiStores.DataAccess.ViewModels;

namespace WebapiStores.Controllers;

[ApiController]
[Route("[controller]")]
public class StoreController: ControllerBase{

    private readonly IStoreRepository _storeRepository;
    private HttpClient _salesHttpClient;

    public StoreController(IStoreRepository storeRepository, IHttpClientFactory clientFactory)
    {
        _storeRepository = storeRepository;
        _salesHttpClient = clientFactory.CreateClient("Sales");

    }
    
    [HttpGet( "GetStoresForDistrict/{districtId}", Name = "GetStoresForDistrict")]
    public ActionResult GetStoresForDistrict(int districtId)
    {
        return Ok(_storeRepository.GetStoresForDistrict(districtId));
    }
    [HttpGet(Name = "GetStores")]
    public ActionResult<IEnumerable<Store>> GetStores()
    {
        return Ok(_storeRepository.GetStores());
    }

    [HttpGet("{storeId}", Name = "GetStore")]
    public ActionResult<Store> GetStore(int storeId)
    {
        var store = _storeRepository.GetStore(storeId);
        if (store == null)
        {
            return NotFound();
        }
        return Ok(store);
    }

    [HttpPost(Name = "AddStore")]
    [ProducesResponseType(typeof(Store), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddStore(AddStore addstore)
    {
        var store = new Store
        {
            StoreName = addstore.StoreName,
            DistrictId = addstore.DistrictId,        
        };
        var getDistrict = await GetDistrict(store.DistrictId);// await _salesHttpClient.GetAsync("/district/" + store.DistrictId);
        if(getDistrict != null){
            store.DistrictName = getDistrict.DistrictName;
            _storeRepository.AddStore(store);
            return CreatedAtRoute("GetStore", new { storeId = store.StoreId }, store);
        }
        else
            return BadRequest("District not found");
    }

    private async Task<District?> GetDistrict(int districtId)
    {
        var getDistrict = await _salesHttpClient.GetAsync("/district/" + districtId);
        var district = await getDistrict.Content.ReadFromJsonAsync<District>();
        if(getDistrict.IsSuccessStatusCode)
            return district;

        return null;
    }

    [HttpPut("{storeId}", Name = "UpdateStore")]
    [ProducesResponseType(typeof(Store), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateStore(int storeId, UpdateStore store)
    {
        if (storeId != store.StoreId)
        {
            return BadRequest();
        }
        if (!_storeRepository.StoreExists(storeId))
        {
            return NotFound();
        }
        var getDistrict = await GetDistrict(store.DistrictId);// await _salesHttpClient.GetAsync("/district/" + store.DistrictId);
        if(getDistrict == null){
            return BadRequest("District not found");
        }
        var updatestore = new Store
        {
            StoreId = store.StoreId,
            StoreName = store.StoreName,
            DistrictId = store.DistrictId,
            DistrictName = getDistrict.DistrictName
        };
        _storeRepository.UpdateStore(updatestore);
        return Ok(updatestore);
    }

    [HttpDelete("{storeId}", Name = "DeleteStore")]
    public ActionResult<Store> DeleteStore(int storeId)
    {
        if (!_storeRepository.StoreExists(storeId))
        {
            return NotFound();
        }
        _storeRepository.DeleteStore(storeId);
        return NoContent();
    }
}