using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Models.Enums;
using Xunit;

namespace ContractMonthlyClaimSystem.Tests.Models
{
	public class UserTests
	{
		[Fact]
		public void User_Should_Require_FirstName()
		{
			// Arrange
			var user = new User { LastName = "Doe", Email = "john@example.com" }; // Set other properties to non-null values

			// Act
			var validationResults = ValidateModel(user).ToList();

			// Assert
			Assert.Contains(validationResults, vr => vr.MemberNames.Contains("FirstName"));
			Assert.Equal("First name is required", validationResults[0].ErrorMessage);
		}

		[Fact]
		public void User_Should_Require_LastName()
		{
			// Arrange
			var user = new User { FirstName = "John", Email = "john@example.com" }; // Set other properties to non-null values

			// Act
			var validationResults = ValidateModel(user).ToList();

			// Assert
			Assert.Contains(validationResults, vr => vr.MemberNames.Contains("LastName"));
			Assert.Equal("Last name is required", validationResults[0].ErrorMessage);
		}


		[Fact]
		public void User_Should_Require_Valid_Email()
		{
			// Arrange
			var user = new User { FirstName = "John", LastName = "Doe", Email = "invalid-email" };

			// Act
			var validationResults = ValidateModel(user).ToList();

			// Assert
			Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Email"));
			Assert.Equal("Invalid email address", validationResults[0].ErrorMessage);
		}

		[Fact]
		public void User_Should_Have_FullName_Property()
		{
			// Arrange
			var user = new User { FirstName = "John", LastName = "Doe" };

			// Act
			var fullName = user.FullName;

			// Assert
			Assert.Equal("John Doe", fullName);
		}

		[Fact]
		public void User_Should_Default_Role_To_Lecturer()
		{
			// Arrange
			var user = new User();

			// Act & Assert
			Assert.Equal(Role.Lecturer, user.Role);
		}

		private static ICollection<ValidationResult> ValidateModel(User user)
		{
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(user);
			Validator.TryValidateObject(user, validationContext, validationResults, true);
			return validationResults;
		}
	}
}

