using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContractMonthlyClaimSystem.Models;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace ContractMonthlyClaimSystem.Tests
{
	public class SupportingDocumentTests
	{
		private readonly SupportingDocumentValidator _validator;

		public SupportingDocumentTests()
		{
			_validator = new SupportingDocumentValidator();
		}

		[Fact]
		public void Should_Have_Error_When_ClaimId_Is_Zero()
		{
			var document = new SupportingDocument
			{
				ClaimId = 0,
						FileName = "test.pdf",
						FilePath = "/path/to/test.pdf",
						FileSize = 500
			};

			var result = _validator.TestValidate(document);
			result.ShouldHaveValidationErrorFor(doc => doc.ClaimId);
		}

		[Fact]
		public void Should_Have_Error_When_FileName_Is_Null()
		{
			var document = new SupportingDocument
			{
				ClaimId = 1,
				// FileName was set to null when defined in class
				FilePath = "/path/to/test.pdf",
				FileSize = 500
			};

			var result = _validator.TestValidate(document);
			result.ShouldHaveValidationErrorFor(doc => doc.FileName);
		}

		[Fact]
		public void Should_Have_Error_When_FilePath_Is_Null()
		{
			var document = new SupportingDocument
			{
				ClaimId = 1,
				FileName = "test.pdf",
				// FilePath was set to null when defined in class
				FileSize = 500
			};

			var result = _validator.TestValidate(document);
			result.ShouldHaveValidationErrorFor(doc => doc.FilePath);
		}

		[Fact]
		public void Should_Have_Error_When_FileSize_Is_Too_Small()
		{
			var document = new SupportingDocument
			{
				ClaimId = 1,
						FileName = "test.pdf",
						FilePath = "/path/to/test.pdf",
						FileSize = 0
			};

			var result = _validator.TestValidate(document);
			result.ShouldHaveValidationErrorFor(doc => doc.FileSize);
		}

		[Fact]
		public void Should_Have_Error_When_FileSize_Is_Too_Large()
		{
			var document = new SupportingDocument
			{
				ClaimId = 1,
						FileName = "test.pdf",
						FilePath = "/path/to/test.pdf",
						FileSize = 10 * 1024 * 1024 + 1 // 10 MB + 1 byte
			};

			var result = _validator.TestValidate(document);
			result.ShouldHaveValidationErrorFor(doc => doc.FileSize);
		}

		[Fact]
		public void Should_Not_Have_Error_When_Valid()
		{
			var document = new SupportingDocument
			{
				ClaimId = 1,
						FileName = "test.pdf",
						FilePath = "/path/to/test.pdf",
						FileSize = 500 // Valid size
			};

			var result = _validator.TestValidate(document);
			result.ShouldNotHaveAnyValidationErrors();
		}
	}

	public class SupportingDocumentValidator : AbstractValidator<SupportingDocument>
	{
		public SupportingDocumentValidator()
		{
			RuleFor(doc => doc.ClaimId).GreaterThan(0);
			RuleFor(doc => doc.FileName).NotEmpty();
			RuleFor(doc => doc.FilePath).NotEmpty();
			RuleFor(doc => doc.FileSize).InclusiveBetween(1, 10 * 1024 * 1024);
		}
	}
}
