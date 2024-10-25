using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ContractMonthlyClaimSystem.Validation
{
	public class MaxFileSizeAttribute : ValidationAttribute
	{
		private readonly int _maxFileSize;

		public MaxFileSizeAttribute(int maxFileSize)
		{
			_maxFileSize = maxFileSize;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is IFormFileCollection files)
			{
				foreach (var file in files)
				{
					if (file.Length > _maxFileSize)
					{
						return new ValidationResult(ErrorMessage ?? $"File size should not exceed {_maxFileSize / (1024 * 1024)} MB.");
					}
				}
			}
			return ValidationResult.Success;
		}
	}
}