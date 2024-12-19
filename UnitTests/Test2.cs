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
    public class FinancingOptionsControllerTests
    {
        private Mock<IFinancingOptionsService> _mockService;
        private FinancingOptionsController _controller;
        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IFinancingOptionsService>();
            _controller = new FinancingOptionsController(_mockService.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithProperties()
        {
            // Arrange
            int buildId = 1;
            var properties = new List<FinancingOption> { new FinancingOption { Id = 1, NewBuildingId = buildId } };
            _mockService.Setup(s => s.GetAllFinancingOptionsAsync(buildId)).ReturnsAsync(properties);

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
            var model = new FinancingOption { Caption = "Test", Description = "description", NewBuildingId = 1 };
            _mockService.Setup(s => s.CreateFinancingOptionAsync(model.Caption, model.Description, model.NewBuildingId)).ReturnsAsync(true);

            // Act
            var result = (RedirectToActionResult)await _controller.Create(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(model.NewBuildingId, result.RouteValues["buildId"]);
        }

        [TestMethod]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var initialFinancingOption = new FinancingOption { Id = 1, Caption = "Test", Description = "description", NewBuildingId = 1 };
            var updatedFinancingOption = new FinancingOption { Id = 1, Caption = "Test1", Description = "description1", NewBuildingId = 1 };

            FinancingOption actualUpdatedFinancingOption = null;

            _mockService.Setup(s => s.GetFinancingOptionAsync(initialFinancingOption.Id)).ReturnsAsync(initialFinancingOption);

            _mockService
                .Setup(s => s.UpdateFinancingOptionAsync(
                 It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()
                ))
                .Callback<int, string, string, int>((id, caption, description, newBuildingId) =>
                {
                    actualUpdatedFinancingOption = new FinancingOption
                    {
                        Id = id,
                        Caption = caption,
                        Description = description,
                        NewBuildingId = newBuildingId,
                    };
                })
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(updatedFinancingOption) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(updatedFinancingOption.NewBuildingId, result.RouteValues["buildId"]);

            Assert.IsNotNull(actualUpdatedFinancingOption);
            Assert.AreEqual(updatedFinancingOption.Id, actualUpdatedFinancingOption.Id);
            Assert.AreEqual(updatedFinancingOption.Caption, actualUpdatedFinancingOption.Caption);
            Assert.AreEqual(updatedFinancingOption.NewBuildingId, actualUpdatedFinancingOption.NewBuildingId);
        }

        [TestMethod]
        public async Task Delete_Process_DeletesFinancingOptionAndRedirectsToIndex()
        {
            // Arrange
            var FinancingOptionId = 1;
            var FinancingOption = new FinancingOption { Id = 1, Caption = "Test", Description = "description", NewBuildingId = 1 };

            _mockService.Setup(s => s.GetFinancingOptionAsync(FinancingOptionId)).ReturnsAsync(FinancingOption);
            _mockService.Setup(s => s.DeleteFinancingOptionAsync(FinancingOptionId)).ReturnsAsync(true);

            var getResult = await _controller.Delete(FinancingOptionId) as ViewResult;
            Assert.IsNotNull(getResult);
            Assert.AreEqual(FinancingOption, getResult.Model);
            _mockService.Verify(s => s.GetFinancingOptionAsync(FinancingOptionId), Times.Once);


            var postResult = await _controller.DeleteConfirmed(FinancingOptionId) as RedirectToActionResult;
            Assert.IsNotNull(postResult);
            Assert.AreEqual("Index", postResult.ActionName);
            Assert.AreEqual(FinancingOption.NewBuildingId, postResult.RouteValues["buildId"]);
        }
    }
}

