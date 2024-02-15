using Microsoft.AspNetCore.Mvc;
using webapi.DataAccess.Interfaces;
using webapi.DataAccess.Models;
using webapi.DataAccess.ViewModels;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class DistrictController : ControllerBase
{
    private readonly IDistrictRepository _districtRepository;
    private readonly ILogger<WeatherForecastController> _logger;
    public DistrictController(IDistrictRepository districtRepository, ILogger<WeatherForecastController> logger)
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
    public ActionResult<District> AddDistrict(District district)
    {
        try
        {
            _districtRepository.AddDistrict(district);
            return CreatedAtRoute("GetDistrict", new { districtId = district.DistrictId }, district);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"error");
            return StatusCode(500, e.Message);
        }
        
    }

    [HttpPut("{districtId}", Name = "UpdateDistrict")]
    public ActionResult UpdateDistrict(int districtId, District district)
    {
        if (districtId != district.DistrictId)
        {
            return BadRequest();
        }
        _districtRepository.UpdateDistrict(district);
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