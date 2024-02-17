using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebapiSales.Controllers;
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Models;
using WebapiSales.DataAccess.ViewModels;

namespace Sd_Solution.Tests;

public class DistrictControllerTests
{
    private readonly Mock<IDistrictRepository> _mockDistrictRepository;
    private readonly Mock<ISalesPersonRepository> _mockSalesPersonRepository;
    private readonly Mock<ISecondarySalesPersonRepository> _mockSecondarySalesPersonRepository;
    private readonly Mock<ILogger<DistrictController>> _mockLogger;
    private readonly DistrictController _controller;

    public DistrictControllerTests()
    {
        _mockDistrictRepository = new Mock<IDistrictRepository>();
        _mockSalesPersonRepository = new Mock<ISalesPersonRepository>();
        _mockSecondarySalesPersonRepository = new Mock<ISecondarySalesPersonRepository>();
        _mockLogger = new Mock<ILogger<DistrictController>>();
        _controller = new DistrictController(_mockDistrictRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void GetDistricts_ReturnsOkObjectResult()
    {
        _mockDistrictRepository.Setup(x => x.GetDistricts()).Returns(new List<DistrictViewModel>());
        var result = _controller.GetDistricts();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetDistrict_ReturnsOkObjectResult()
    {
        _mockDistrictRepository.Setup(x => x.GetDistrict(1)).Returns(new DistrictViewModel());

        var result = _controller.GetDistrict(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetDistrict_ReturnsNotFoundResult()
    {
        var result = _controller.GetDistrict(0);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void PostDistrict_ReturnsCreatedAtActionResult()
    {
        var district = new AddDistrict
        {
            DistrictName = "Test District",
            PrimarySalesId = 1
        };
        var result = _controller.AddDistrict(district);
        Assert.IsType<CreatedAtRouteResult>(result.Result);
        _mockDistrictRepository.Verify(x => x.AddDistrict(It.IsAny<District>()), Times.Once);
    }

    [Fact]
    public void PutDistrict_ReturnsNoContentResult()
    {
        var district = new UpdateDistrict()
        {
            DistrictId = 1,
            DistrictName = "Test District",
            PrimarySalesId = 1
        };
        var result = _controller.UpdateDistrict(1, district);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void PutDistrict_ReturnsBadRequestResult()
    {
         
        var result = _controller.GetAvailableSalesPersonsForDistrict(2);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void DeleteDistrict_ReturnsNoContentResult()
    {
        var result = _controller.DeleteDistrict(1);
        Assert.IsType<NotFoundResult>(result);
        _mockDistrictRepository.Verify(x => x.DistrictExists(1), Times.Once);
        _mockDistrictRepository.Verify(x => x.DeleteDistrict(1), Times.Never);

    }


}