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
    public class HomeControllerTests
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsers()
        {
            // Arrange
            var mock = new Mock<IUser>();
            mock.Setup(repo => repo.GetAllUsers()).Returns(GetTestUsers());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
            Assert.Equal(GetTestUsers().Count, model.Count());
        }
        private List<User> GetTestUsers()
        {
            var users = new List<User>
            {
                new User { Id=1, Name="Tom", Age=35},
                new User { Id=2, Name="Alice", Age=29},
                new User { Id=3, Name="Sam", Age=32},
                new User { Id=4, Name="Kate", Age=30}
            };
            return users;
        }

        [Fact]
        public void AddUserReturnsViewResultWithUserModel()
        {
            // Arrange
            var mock = new Mock<IUser>();
            var controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            User newUser = new User();

            // Act
            var result = controller.AddUser(newUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newUser, viewResult?.Model);
        }

        [Fact]
        public void AddUserReturnsARedirectAndAddsUser()
        {
            // Arrange
            var mock = new Mock<IUser>();
            var controller = new HomeController(mock.Object);
            var newUser = new User()
            {
                Name = "Ben"
            };

            // Act
            var result = controller.AddUser(newUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.AddUser(newUser));
        }

        [Fact]
        public void GetUserReturnsBadRequestResultWhenIdIsNull()
        {
            // Arrange
            var mock = new Mock<IUser>();
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(null);

            // Arrange
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetUserReturnsNotFoundResultWhenUserNotFound()
        {
            // Arrange
            int testUserId = 1;
            var mock = new Mock<IUser>();
            mock.Setup(repo => repo.GetUser(testUserId))
                .Returns(null as User);
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(testUserId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUserReturnsViewResultWithUser()
        {
            // Arrange
            int testUserId = 1;
            var mock = new Mock<IUser>();
            mock.Setup(repo => repo.GetUser(testUserId))
                .Returns(GetTestUsers().FirstOrDefault(p => p.Id == testUserId));
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(testUserId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.ViewData.Model);
            Assert.Equal("Tom", model.Name);
            Assert.Equal(35, model.Age);
            Assert.Equal(testUserId, model.Id);
        }
        [Fact]
        public void UpdateUserValidModelReturnsRedirectToAction()
        {
            // Arrange
            var userServiceMock = new Mock<IUser>();
            userServiceMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Verifiable();
            var controller = new HomeController(userServiceMock.Object);
            var user = new User { Id = 1, Name = "TestUser" };

            // Act
            var result = controller.UpdateUser(user) as RedirectToActionResult;

            // Assert
            userServiceMock.Verify(x => x.UpdateUser(user), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void UpdateUserInvalidModelReturnsViewResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUser>();
            var controller = new HomeController(userServiceMock.Object);
            var user = new User { Id = 0, Name = "TestUser" };
            controller.ModelState.AddModelError("Id", "Id is required");

            // Act
            var result = controller.UpdateUser(user) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void DeleteUserWithValidIdReturnsViewResult()
        {
            // Arrange
            int userId = 1;
            var mockUsersRepository = new Mock<IUser>();
            mockUsersRepository.Setup(repo => repo.GetUser(userId)).Returns(new User { Id = userId });
            var controller = new HomeController(mockUsersRepository.Object);

            // Act
            var result = controller.DeleteUser(userId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DeleteUserWithInvalidIdReturnsBadRequestResult()
        {
            // Arrange
            int? userId = null;
            var mockUsersRepository = new Mock<IUser>();
            var controller = new HomeController(mockUsersRepository.Object);

            // Act
            var result = controller.DeleteUser(userId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeleteUserWithNonExistingUserReturnsNotFoundResult()
        {
            // Arrange
            int userId = 1;
            var mockUsersRepository = new Mock<IUser>();
            mockUsersRepository.Setup(repo => repo.GetUser(userId)).Returns((User)null);
            var controller = new HomeController(mockUsersRepository.Object);

            // Act
            var result = controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
