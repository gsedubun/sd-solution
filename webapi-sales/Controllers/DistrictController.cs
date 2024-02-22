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

    /// <summary>
    /// Get districts
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetDistricts")]
    [ProducesResponseType(typeof(IEnumerable<DistrictViewModel>), StatusCodes.Status200OK)]
    public ActionResult GetDistricts()
    {
        var districts = _districtRepository.GetDistricts();
        return Ok(districts);
    }

    /// <summary>
    /// Get district
    /// </summary>
    /// <param name="districtId">district id</param>
    /// <returns></returns>
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

    /// <summary>
    /// Add district
    /// </summary>
    /// <param name="district"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddDistrict")]
    [ProducesResponseType(typeof(District), StatusCodes.Status201Created)]
    public ActionResult<District> AddDistrict(AddDistrict district)
    {
        _logger.LogDebug("AddDistrict calling");
            var districtModel = new District()
            {
                DistrictName = district.DistrictName,
                PrimarySalesId = district.PrimarySalesId
            };
            _districtRepository.AddDistrict(districtModel);
            _logger.LogDebug("AddDistrict called");

        return CreatedAtRoute("GetDistrict", new { districtId = districtModel.DistrictId }, districtModel);
       
        
    }

    /// <summary>
    /// Update district
    /// </summary>
    /// <param name="districtId">district id</param>
    /// <param name="district">Update district request body</param>
    /// <returns></returns>
    [HttpPut("{districtId}", Name = "UpdateDistrict")]
    [ProducesResponseType( StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Delete district
    /// </summary>
    /// <param name="districtId">district id</param>
    /// <returns></returns>
    [HttpDelete("{districtId}", Name = "DeleteDistrict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteDistrict(int districtId)
    {
        if (!_districtRepository.DistrictExists(districtId))
        {
            return NotFound();
        }
        _districtRepository.DeleteDistrict(districtId);
        return NoContent();
    }

    /// <summary>
    /// Available sales persons 
    /// </summary>
    /// <remarks>
    /// Get a list of available sales persons for the specific district
    /// </remarks>
    /// <param name="districtId">district id</param>
    /// <returns></returns>
    [HttpGet("{districtId}/AvailableSalesPersons", Name = "GetAvailableSalesPersonsForDistrict")]
    [ProducesResponseType(typeof(IEnumerable<SalesPerson>), StatusCodes.Status200OK)]
    public ActionResult GetAvailableSalesPersonsForDistrict(int districtId)
    {
        var salesPersons = _districtRepository.GetAvailableSalesPersonsForDistrict(districtId);
        return Ok(salesPersons);
    }
}