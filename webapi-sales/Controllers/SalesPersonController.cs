using Microsoft.AspNetCore.Mvc;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;

namespace WebapiSales.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SalesPersonController : Controller
    {
        private readonly ISalesPersonRepository _salesPersonRepository;
        private readonly ILogger<SalesPersonController> _logger;

        public SalesPersonController(ISalesPersonRepository salesPersonRepository,
            ILogger<SalesPersonController> logger)
        {
            _salesPersonRepository = salesPersonRepository;
            _logger = logger;
        }

        [HttpGet(Name = "GetSalesPersons")]
        public ActionResult GetSalesPersons()
        {
            var salesPersons = _salesPersonRepository.GetSalesPersons();
            return Ok(salesPersons);
        }


        [HttpGet("{salesPersonId}", Name = "GetSalesPerson")]
        public ActionResult GetSalesPerson(int salesPersonId)
        {
            var salesPerson = _salesPersonRepository.GetSalesPerson(salesPersonId);
            if (salesPerson == null)
            {
                return NotFound();
            }
            return Ok(salesPerson);
        }

        [HttpPost(Name = "AddSalesPerson")]
        public ActionResult AddSalesPerson(AddSalesPerson salesPerson)
        {
            try
            {
                var salesPersonModel = new SalesPerson()
                {
                    FullName = salesPerson.FullName
                };
                _salesPersonRepository.AddSalesPerson(salesPersonModel);
                return CreatedAtRoute("GetSalesPerson", new { salesPersonId = salesPersonModel.SalesPersonId }, salesPersonModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
                return StatusCode(500, e.Message);
            }
        }

    }
}
