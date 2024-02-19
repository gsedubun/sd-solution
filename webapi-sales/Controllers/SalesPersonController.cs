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

        /// <summary>
        /// Get sales persons
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetSalesPersons")]
        [ProducesResponseType(typeof(IEnumerable<SalesPerson>), StatusCodes.Status200OK)]
        public ActionResult GetSalesPersons()
        {
            var salesPersons = _salesPersonRepository.GetSalesPersons();
            return Ok(salesPersons);
        }

        /// <summary>
        /// Get sales person
        /// </summary>
        /// <param name="salesPersonId"> SalesPerson Id </param>
        /// <returns></returns>
        [HttpGet("{salesPersonId}", Name = "GetSalesPerson")]
        [ProducesResponseType(typeof(SalesPerson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetSalesPerson(int salesPersonId)
        {
            var salesPerson = _salesPersonRepository.GetSalesPerson(salesPersonId);
            if (salesPerson == null)
            {
                return NotFound();
            }
            return Ok(salesPerson);
        }


        /// <summary>
        /// Add sales person
        /// </summary>
        /// <param name="salesPerson">Sales person </param>
        /// <returns></returns>
        [HttpPost(Name = "AddSalesPerson")]
        [ProducesResponseType(typeof(SalesPersonViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddSalesPerson(AddSalesPerson salesPerson)
        {
            try
            {
                var salesPersonModel = new SalesPerson()
                {
                    FullName = salesPerson.FullName
                };
                _salesPersonRepository.AddSalesPerson(salesPersonModel);
                
                return Ok(new AddSalesPerson()
                {
                    FullName = salesPerson.FullName,
                    SalesPersonId = salesPersonModel.SalesPersonId,
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

        /// <summary>
        /// Add SalesPerson to District
        /// </summary>
        /// <param name="salesPerson"> SalesPersonToDistrict  </param>
        /// <returns></returns>
        [HttpPost("AddSalesPersonToDistrict", Name = "AddSalesPersonToDistrict")]
        [ProducesResponseType(typeof(AddSalesPersonToDistrictViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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

        /// <summary>
        /// List sales persons for district
        /// </summary>
        /// <remarks>
        /// Get all available sales person for specific district id
        /// </remarks>
        /// <param name="districtId">district id</param>
        /// <returns></returns>
        [HttpGet("GetForDistrict/{districtId}", Name = "GetSalesPersonsForDistrict")]
        [ProducesResponseType(typeof(IEnumerable<SalesPersonViewModel>), StatusCodes.Status200OK)]
        public ActionResult GetSalesPersonsForDistrict(int districtId)
        {
            var secondarySalesPersons = _secondarySalesPersonRepository.GetSecondarySalesPersonByDistrictId(districtId).ToList();
            var district = _districtRepository.GetDistrict(districtId);

            List<SalesPersonViewModel> salesPersonsOrdered = new List<SalesPersonViewModel>(secondarySalesPersons.Count + 1);
            salesPersonsOrdered.Add(new SalesPersonViewModel(district!));
            salesPersonsOrdered.AddRange(secondarySalesPersons);

            return Ok(salesPersonsOrdered);
        }


        /// <summary>
        /// Delete secondary sales person
        /// </summary>
        /// <param name="salesPerson">SalesPerson to delete</param>
        /// <returns></returns>
        [HttpDelete("deleteSalesPersonDistrict")]
        [ProducesResponseType(typeof(SalesPersonViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
