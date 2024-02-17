using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebapiSales.Controllers;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.ViewModels;

namespace Sd_Solution.Tests
{
    public class SalesPersonControllerTests
    {
        private readonly Mock<ISalesPersonRepository> _mockSalesPersonRepository;
        private readonly Mock<IDistrictRepository> _mockDistrictRepository;
        private readonly Mock<ISecondarySalesPersonRepository> _mockSecondarySalesRepository; 
        private readonly Mock<ILogger<SalesPersonController>> _mockLogger;
        private readonly SalesPersonController _controller;
        public SalesPersonControllerTests()
        {
            _mockSalesPersonRepository = new Mock<ISalesPersonRepository>();
            _mockDistrictRepository = new Mock<IDistrictRepository>();
            _mockSecondarySalesRepository = new Mock<ISecondarySalesPersonRepository>();
            _mockLogger = new Mock<ILogger<SalesPersonController>>();
            _controller = new SalesPersonController(_mockSalesPersonRepository.Object, _mockLogger.Object,
               _mockDistrictRepository.Object, _mockSecondarySalesRepository.Object );
        }

        [Fact]
        public void AddSalesPersonToDistrict_ReturnNotFound()
        {
            var salesPerson = new AddSalesPersonToDistrictViewModel
            {
                DistrictId = 1,
                SalesPersonId = 1,
            };
            _mockDistrictRepository.Setup(x => x.GetDistrict(salesPerson.DistrictId)).Returns(() => null);
            var result = _controller.AddSalesPersonToDistrict(salesPerson);
            
            Assert.IsType<NotFoundObjectResult>(result);

        }
    }
}