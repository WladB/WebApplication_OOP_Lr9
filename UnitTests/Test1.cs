using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_OOP_Lr9.Controllers;
using WebApp_OOP_Lr9.DataBase;
using WebApp_OOP_Lr9.Servises;


namespace UnitTests
{
    [TestClass]
    public class PropertiesControllerTests
    {
        private Mock<IPropertiesService> _mockService;
        private PropertiesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IPropertiesService>();
            _controller = new PropertiesController(_mockService.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithProperties()
        {
            // Arrange
            int buildId = 1;
            var properties = new List<Property> { new Property { Id = 1, NewBuildingId = buildId } };
            _mockService.Setup(s => s.GetAllPropertyAsync(buildId)).ReturnsAsync(properties);

            // Act
            var result = (ViewResult)await _controller.Index(buildId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(properties, result.Model);
        }

        [TestMethod]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new Property { CountRooms = 2, NewBuildingId = 1, Area = 50, Floor = 2 };
            _mockService.Setup(s => s.CreatePropertyAsync(model.CountRooms, model.NewBuildingId, model.Area, model.Floor)).ReturnsAsync(true);

            // Act
            var result = (RedirectToActionResult)await _controller.CreateConfirmed(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(model.NewBuildingId, result.RouteValues["buildId"]);
        }

        [TestMethod]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var initialProperty = new Property { Id = 1, CountRooms = 2, NewBuildingId = 1, Area = 50.0f, Floor = 2 };
            var updatedProperty = new Property { Id = 1, CountRooms = 3, NewBuildingId = 1, Area = 70.5f, Floor = 4 };

            Property actualUpdatedProperty = null;

            _mockService.Setup(s => s.GetPropertyAsync(initialProperty.Id)).ReturnsAsync(initialProperty);

            _mockService
                .Setup(s => s.UpdatePropertyAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<float>(), It.IsAny<byte>()
                ))
                .Callback<int, int, int, float, byte>((id, countRooms, newBuildingId, area, floor) =>
                {
                    actualUpdatedProperty = new Property
                    {
                        Id = id,
                        CountRooms = countRooms,
                        NewBuildingId = newBuildingId,
                        Area = area,
                        Floor = floor
                    };
                })
                .ReturnsAsync(true);

            // Act
            var result = await _controller.EditConfirmed(updatedProperty) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(updatedProperty.NewBuildingId, result.RouteValues["buildId"]);

            Assert.IsNotNull(actualUpdatedProperty);
            Assert.AreEqual(updatedProperty.Id, actualUpdatedProperty.Id);
            Assert.AreEqual(updatedProperty.CountRooms, actualUpdatedProperty.CountRooms);
            Assert.AreEqual(updatedProperty.NewBuildingId, actualUpdatedProperty.NewBuildingId);
            Assert.AreEqual(updatedProperty.Area, actualUpdatedProperty.Area);
            Assert.AreEqual(updatedProperty.Floor, actualUpdatedProperty.Floor);
        }

        [TestMethod]
        public async Task Delete_Process_DeletesPropertyAndRedirectsToIndex()
        {
            // Arrange
            var propertyId = 1;
            var property = new Property { Id = propertyId, CountRooms = 3, Area = 70.5f, NewBuildingId = 2 };

            _mockService.Setup(s => s.GetPropertyAsync(propertyId)).ReturnsAsync(property);
            _mockService.Setup(s => s.DeletePropertyAsync(propertyId)).ReturnsAsync(true);

            var getResult = await _controller.Delete(propertyId) as ViewResult;
            Assert.IsNotNull(getResult);
            Assert.AreEqual(property, getResult.Model);
            _mockService.Verify(s => s.GetPropertyAsync(propertyId), Times.Once);


            var postResult = await _controller.DeleteConfirmed(propertyId) as RedirectToActionResult;
            Assert.IsNotNull(postResult);
            Assert.AreEqual("Index", postResult.ActionName);
            Assert.AreEqual(property.NewBuildingId, postResult.RouteValues["buildId"]);
        }
    }
   
}
