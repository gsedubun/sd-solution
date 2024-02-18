using Microsoft.AspNetCore.Mvc;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;

namespace WebapiSales.Controllers;

[ApiController]
[Route("[controller]")]
public class DistrictController : ControllerBase
{
    private readonly ISecondarySalesPersonRepository _secondarySalesRepository;

    private readonly IDistrictRepository _districtRepository;
    private readonly ILogger<DistrictController> _logger;
    public DistrictController(IDistrictRepository districtRepository, ILogger<DistrictController> logger,ISecondarySalesPersonRepository secondarySalesRepository)
    {
        _districtRepository = districtRepository;
        _logger = logger;
        _secondarySalesRepository = secondarySalesRepository;
    }

    [HttpGet(Name = "GetDistricts")]
    [ProducesResponseType(typeof(IEnumerable<DistrictViewModel>), StatusCodes.Status200OK)]
    public ActionResult GetDistricts()
    {
        var districts = _districtRepository.GetDistricts();
        return Ok(districts);
    }

    [HttpGet("{districtId}", Name = "GetDistrict")]
    [ProducesResponseType(typeof(DistrictViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetDistrict(int districtId)
    {
        var district = _districtRepository.GetDistrict(districtId);
        if (district == null)
        {
            return NotFound();
        }
        return Ok(district);
    }

    [HttpPost(Name = "AddDistrict")]
    [ProducesResponseType(typeof(District), StatusCodes.Status201Created)]
    public ActionResult<District> AddDistrict(AddDistrict district)
    {
            var districtModel = new District()
            {
                DistrictName = district.DistrictName,
                PrimarySalesId = district.PrimarySalesId
            };
            _districtRepository.AddDistrict(districtModel);
            return CreatedAtRoute("GetDistrict", new { districtId = districtModel.DistrictId }, districtModel);
       
        
    }

    [HttpPut("{districtId}", Name = "UpdateDistrict")]
    public ActionResult UpdateDistrict(int districtId, UpdateDistrict district)
    {
        if (districtId != district.DistrictId)
        {
            return BadRequest();
        }
        var districtModel = new District()
        {
            DistrictId = district.DistrictId,
            DistrictName = district.DistrictName,
            PrimarySalesId = district.PrimarySalesId
        };
        _districtRepository.UpdateDistrict(districtModel);

        //Delete secondary sales person if exists for the district
        if (_secondarySalesRepository.SecondarySalesPersonExists(districtModel.PrimarySalesId, districtId))
        {
            _secondarySalesRepository.DeleteSecondarySalesPerson(districtModel.PrimarySalesId, districtId);
        }

        return NoContent();
    }

    [HttpDelete("{districtId}", Name = "DeleteDistrict")]
    public ActionResult DeleteDistrict(int districtId)
    {
        if (!_districtRepository.DistrictExists(districtId))
        {
            return NotFound();
        }
        _districtRepository.DeleteDistrict(districtId);
        return NoContent();
    }

    [HttpGet("{districtId}/AvailableSalesPersons", Name = "GetAvailableSalesPersonsForDistrict")]
    [ProducesResponseType(typeof(IEnumerable<SalesPerson>), StatusCodes.Status200OK)]
    public ActionResult GetAvailableSalesPersonsForDistrict(int districtId)
    {
        var salesPersons = _districtRepository.GetAvailableSalesPersonsForDistrict(districtId);
        return Ok(salesPersons);
    }
}