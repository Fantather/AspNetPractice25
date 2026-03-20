using AspNetPractice25.Controllers;
using AspNetPractice25.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AspNetPractice25.Tests
{
    public class OrdersControllerTests
    {
        [Fact]
        public void Add_RedirectsToIndex_AndCreatesOrder_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();
            var controller = new OrdersController(mockRepo.Object);
            var newOrder = new Order();

            // Act
            var result = controller.Add(newOrder);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify(repo => repo.CreateOrder(newOrder), Times.Once);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenOrderIsNull()
        {
            int testId = 1;
            var mockRepo = new Mock<IOrderRepository>();
            var controller = new OrdersController(mockRepo.Object);

            var result = controller.Details(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
