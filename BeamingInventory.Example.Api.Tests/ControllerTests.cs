using System;
using BeamingInventory.Example.Api.Controllers;
using BeamingInventory.Example.Api.Models;
using BeamingInventory.Example.Api.Services;
using BeamingInventory.Example.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BeamingInventory.Example.Api.Tests
{
    public class ControllerTests
    {
        [Fact]
        public void TestPostSales()
        {
            var inventoryManagerMock = new Mock<IInventoryManager>();
            var expectedValues = new InventoryChange { PreviousCount = 2, CurrentCount = 0 };
            inventoryManagerMock.Setup(a => a.Sell(It.IsAny<int>())).Returns(expectedValues);
            var controller = new SalesController(inventoryManagerMock.Object);
            var vm = new PostSaleVm { Count = 2 };

            var actionResult = controller.PostSale(vm);
            var result = actionResult as OkObjectResult;
            var value = result?.Value as InventoryChange;

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<InventoryChange>(value);

            Assert.Equal(expectedValues.PreviousCount, value.PreviousCount);
            Assert.Equal(expectedValues.CurrentCount, value.CurrentCount);
        }

        [Fact]
        public void AssertPostSalesMethodIsPost()
        {
            var action = typeof(SalesController).GetMethod(nameof(SalesController.PostSale));
            var attribute = typeof(HttpPostAttribute);

            var isDefined = Attribute.IsDefined(action, attribute);

            Assert.True(isDefined);
        }
    }
}
