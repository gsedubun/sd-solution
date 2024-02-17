using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.Repositories;
using WebapiSales.DataAccess.ViewModels;

namespace WebapiSales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesPersonController : Controller
    {
        private readonly ISalesPersonRepository _salesPersonRepository;
        private readonly ILogger<SalesPersonController> _logger;
        private readonly IDistrictRepository _districtRepository;
        private readonly ISecondarySalesPersonRepository _secondarySalesPersonRepository;

        public SalesPersonController(ISalesPersonRepository salesPersonRepository,
            ILogger<SalesPersonController> logger, IDistrictRepository districtRepository, ISecondarySalesPersonRepository secondarySalesPersonRepository)
        {
            _salesPersonRepository = salesPersonRepository;
            _logger = logger;
            _districtRepository = districtRepository;
            _secondarySalesPersonRepository = secondarySalesPersonRepository;
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


                return Ok(new SalesPersonViewModel()
                {
                    DistrictId = salesPerson.DistrictId,
                    FullName = salesPerson.FullName,
                    SalesPersonId = salesPersonModel.SalesPersonId,
                    SalesType = salesPerson.SalesType
                });
            }
            catch (SqlException e)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("AddSalesPersonToDistrict", Name = "AddSalesPersonToDistrict")]
        public ActionResult AddSalesPersonToDistrict(AddSalesPersonToDistrictViewModel salesPerson)
        {
            var district = _districtRepository.GetDistrict(salesPerson.DistrictId);
            if (district == null)
            {
                return NotFound("District does not exist");
            }
            if (salesPerson.SalesType == "Primary" && salesPerson.DistrictId != 0)
            {

                var districtModel = new District()
                {
                    DistrictName = district!.DistrictName,
                    PrimarySalesId = salesPerson.SalesPersonId,
                    DistrictId = district.DistrictId
                };
                _districtRepository.UpdateDistrict(districtModel);
            }
            else if (salesPerson.SalesType == "Secondary" && salesPerson.DistrictId != 0)
            {
                var secondarySalesPerson = new SecondarySalesPerson()
                {
                    SalesPersonId = salesPerson.SalesPersonId,
                    DistrictId = salesPerson.DistrictId
                };
                _secondarySalesPersonRepository.AddSecondarySalesPerson(secondarySalesPerson);
            }

            return Ok(salesPerson);
        }

        [HttpGet("GetForDistrict/{districtId}", Name = "GetSalesPersonsForDistrict")]
        public ActionResult GetSalesPersonsForDistrict(int districtId)
        {
            var salesPersons = _secondarySalesPersonRepository.GetSecondarySalesPersonByDistrictId(districtId).ToList();
            var district = _districtRepository.GetDistrict(districtId);

            List<SalesPersonViewModel> salesPersonsOrdered = new List<SalesPersonViewModel>(salesPersons.Count + 1);
            salesPersonsOrdered.Add(new SalesPersonViewModel(district!));
            salesPersonsOrdered.AddRange(salesPersons);

            return Ok(salesPersonsOrdered);
        }

        [HttpDelete("deleteSalesPersonDistrict")]
        public ActionResult DeleteSalesPersonDistrict(SalesPersonViewModel salesPerson)
        {
            if (salesPerson.SalesType == "Secondary" && salesPerson.DistrictId != 0)
            {
                _secondarySalesPersonRepository.DeleteSecondarySalesPerson(salesPerson.SalesPersonId,
                    salesPerson.DistrictId);
                return Ok(salesPerson);

            }
            else
            {
                return BadRequest("Cannot delete primary sales person.");
            }

        }
    }
}
