using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ContractMonthlyClaimSystem.Validation
{
	public class AllowedExtensionsAttribute : ValidationAttribute
	{
		private readonly string[] _extensions;

		public AllowedExtensionsAttribute(string[] extensions)
		{
			_extensions = extensions;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is IFormFileCollection files)
			{
				foreach (var file in files)
				{
					var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
					if (!_extensions.Contains(extension))
					{
						return new ValidationResult(ErrorMessage ?? "Invalid file type.");
					}
				}
			}
			return ValidationResult.Success;
		}
	}
}
