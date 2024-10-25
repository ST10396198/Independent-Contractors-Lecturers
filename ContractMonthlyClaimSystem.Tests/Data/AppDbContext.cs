using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace ContractMonthlyClaimSystem.Tests
{
    public class AppDbContextTests
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Generate a unique name for the in-memory database
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return optionsBuilder.Options;
        }

        [Fact]
        public void Can_Add_User_To_Database()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);
            var user = new User { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };

            // Act
            context.User.Add(user);
            context.SaveChanges();

            // Assert
            Assert.Equal(1, context.User.Count());
            Assert.Equal("Jane", context.User.First().FirstName); // Fixed from "John" to "Jane"
        }

        [Fact]
        public void Can_Add_MonthlyClaim_With_User()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new AppDbContext(options);
            var user = new User { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            context.User.Add(user);
            context.SaveChanges();

            var claim = new MonthlyClaim
            {
                UserId = user.Id,
                HoursWorked = 5,
                HourlyRate = 100,
                SubmissionDate = DateTime.Now,
                Status = Status.Pending,
                Description = "Monthly work hours",
                Course = Course.Course1
            };

            // Act
            context.Claims.Add(claim);
            context.SaveChanges();

            // Assert
            Assert.Equal(1, context.Claims.Count());
            Assert.Equal(user.Id, context.Claims.First().UserId);
        }
    }
}

