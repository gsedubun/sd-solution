using Microsoft.AspNetCore.Mvc;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;

namespace WebapiSales.Controllers;

[ApiController]
[Route("[controller]")]
public class SecondarySalesPersonController : Controller
{
    private readonly ISecondarySalesPersonRepository _secondarySalesPersonRepository;
    private readonly ISalesPersonRepository _salesPersonRepository;
    private readonly ILogger<SalesPersonController> _logger;
    private readonly IDistrictRepository _districtRepository;

    public SecondarySalesPersonController(ISecondarySalesPersonRepository secondarySalesPersonRepository, ILogger<SalesPersonController> logger,
        IDistrictRepository districtRepository,
        ISalesPersonRepository salesPersonRepository)
    {
        _secondarySalesPersonRepository = secondarySalesPersonRepository;
        _logger = logger;
        _districtRepository = districtRepository;
        _salesPersonRepository = salesPersonRepository;
    }
    

    [HttpPost(Name = "AddSecondarySalesPerson")]
    public ActionResult AddSecondarySalesPerson(AddSecondarySalesPerson secondarySalesPerson)
    {

        var secondarySalesPersonModel = new SecondarySalesPerson()
        {
            SalesPersonId = secondarySalesPerson.SalesPersonId,
            DistrictId = secondarySalesPerson.DistrictId
        };

        var districtExists = _districtRepository.DistrictExists(secondarySalesPersonModel.DistrictId);
        if (!districtExists)
        {
            return NotFound("District does not exist");
        }

        var salesPersonExists = _salesPersonRepository.SalesPersonExists(secondarySalesPersonModel.SalesPersonId);
        if (!salesPersonExists)
        {
            return NotFound("SalesPerson does not exist");
        }
        _secondarySalesPersonRepository.AddSecondarySalesPerson(secondarySalesPersonModel);
        return CreatedAtRoute("GetSecondarySalesPersons", new { districtId = secondarySalesPersonModel.DistrictId },
            secondarySalesPersonModel);

    }
}