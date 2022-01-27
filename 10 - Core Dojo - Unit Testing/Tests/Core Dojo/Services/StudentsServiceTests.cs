using Common.RequestModels;
using Core_Dojo.Services;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.ResponseModels;
using Moq;
using Repositories.Repositories;
using Xunit;

namespace Tests.Core_Dojo.Services
{
    public class StudentsServiceTests
    {
        [Fact]
        public async Task AddStudent_WhenAddStudentRequestIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = new StudentsService(null, null);
            StudentAddRequest student = null;

            // Act
            Func<Task<bool>> act = () => service.AddStudent(student);

            // Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
            Assert.Contains("request", exception.Message);
        }

        [Fact]
        public async Task AddStudent_WhenValidRequest_ShouldReturnBoolean()
        {
            // Arrange
            var studentsRepositoryMock = new Mock<IStudentsRepository>();
            var service = new StudentsService(studentsRepositoryMock.Object, null);
            var student1 = new StudentAddRequest
            {
                FirstName = "Joe",
                MiddleNames = "Fred",
                Surname = "Bloggs",
                DateOfBirth = new DateTime(1980, 10, 24)
            };
            var student2 = new StudentAddRequest
            {
                FirstName = "Joe",
                MiddleNames = "Fred",
                Surname = "Bloggs",
                DateOfBirth = new DateTime(1980, 10, 24)
            };

            studentsRepositoryMock.Setup(sr => sr.AddStudent(student1)).ReturnsAsync(true);

            // Act
            var result = await service.AddStudent(student2);

            // Assert
            Assert.True(result);

            studentsRepositoryMock.Verify(sr => sr.AddStudent(student1), Times.Once);
        }
    }
}
