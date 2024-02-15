using Microsoft.AspNetCore.Mvc;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;

namespace WebapiSales.Controllers;

[ApiController]
[Route("[controller]")]
public class DistrictController : ControllerBase
{
    private readonly IDistrictRepository _districtRepository;
    private readonly ILogger<DistrictController> _logger;
    public DistrictController(IDistrictRepository districtRepository, ILogger<DistrictController> logger)
    {
        _districtRepository = districtRepository;
        _logger = logger;
    }

    [HttpGet(Name = "GetDistricts")]
    [ProducesResponseType(typeof(IEnumerable<District>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<District>> GetDistricts()
    {
        var districts = _districtRepository.GetDistricts();
        return Ok(districts);
    }

    [HttpGet("{districtId}", Name = "GetDistrict")]
    [ProducesResponseType(typeof(District), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<District> GetDistrict(int districtId)
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
        try
        {
            var districtModel = new District()
            {
                DistrictName = district.DistrictName,
                PrimarySalesId = district.PrimarySalesId
            };
            _districtRepository.AddDistrict(districtModel);
            return CreatedAtRoute("GetDistrict", new { districtId = districtModel.DistrictId }, districtModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"error");
            return StatusCode(500, e.Message);
        }
        
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
}