using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using FluentAssertions;
using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Models.Enums;

namespace ContractMonthlyClaimSystem.Tests
{
	public class MonthlyClaimTests
	{
		[Fact]
		public void MonthlyClaim_ShouldBeValid_WhenAllPropertiesAreSetCorrectly()
		{
			// Arrange
			var monthlyClaim = new MonthlyClaim
			{
				UserId = 1,
				Status = Status.Pending,
				SubmissionDate = DateTime.Now,
				HoursWorked = 10,
				HourlyRate = 20,
				Description = "Worked on project",
				Course = Course.Course2,
				SupportingDocuments = new List<SupportingDocument>()
			};

			// Act
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(monthlyClaim);
			var isValid = Validator.TryValidateObject(monthlyClaim, validationContext, validationResults, true);

			// Assert
			isValid.Should().BeTrue();
			validationResults.Should().BeEmpty();
		}

		[Fact]
		public void MonthlyClaim_ShouldBeInvalid_WhenHoursWorkedIsZero()
		{
			// Arrange
			var monthlyClaim = new MonthlyClaim
			{
				UserId = 1,
				Status = Status.Pending,
				SubmissionDate = DateTime.Now,
				HoursWorked = 0,
				HourlyRate = 20,
				Description = "Worked on project",
				Course = Course.Course1
			};

			// Act
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(monthlyClaim);
			var isValid = Validator.TryValidateObject(monthlyClaim, validationContext, validationResults, true);

			// Assert
			isValid.Should().BeFalse();
			validationResults.Should().ContainSingle()
				.Which.ErrorMessage.Should().Be("Hours worked must be greater than zero.");
		}

		[Fact]
		public void MonthlyClaim_ShouldBeInvalid_WhenHourlyRateIsZero()
		{
			// Arrange
			var monthlyClaim = new MonthlyClaim
			{
				UserId = 1,
					Status = Status.Pending,
					SubmissionDate = DateTime.Now,
					HoursWorked = 10,
					HourlyRate = 0,
					Description = "Worked on project",
					Course = Course.Course1
			};

			// Act
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(monthlyClaim);
			var isValid = Validator.TryValidateObject(monthlyClaim, validationContext, validationResults, true);

			// Assert
			isValid.Should().BeFalse();
			validationResults.Should().ContainSingle()
				.Which.ErrorMessage.Should().Be("Hourly rate must be greater than zero.");
		}

		[Fact]
		public void TotalAmount_ShouldReturnCorrectValue()
		{
			// Arrange
			var monthlyClaim = new MonthlyClaim { HoursWorked = 10, HourlyRate = 20 };

			// Act
			var totalAmount = monthlyClaim.TotalAmount;

			// Assert
			totalAmount.Should().Be(200);
		}
	}
}
