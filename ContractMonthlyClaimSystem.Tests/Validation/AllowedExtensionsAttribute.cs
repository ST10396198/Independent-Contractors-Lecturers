using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using ContractMonthlyClaimSystem.Validation;

namespace ContractMonthlyClaimSystem.Tests.Validation
{
    public class AllowedExtensionsAttributeTests
    {
        [Fact]
        public void IsValid_WithValidExtensions_ReturnsSuccess()
        {
            // Arrange
            var allowedExtensions = new[] { ".jpg", ".png" };
            var attribute = new AllowedExtensionsAttribute(allowedExtensions)
            {
                ErrorMessage = "Invalid file type."
            };

            var files = new Mock<IFormFileCollection>();
            var fileList = new List<IFormFile>
            {
                CreateMockFile("image1.jpg"),
                CreateMockFile("image2.png")
            };
            files.Setup(f => f.GetEnumerator()).Returns(fileList.GetEnumerator());

            // Act
            var result = attribute.GetValidationResult(files.Object, new ValidationContext(files.Object));

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithInvalidExtension_ReturnsValidationResult()
        {
            // Arrange
            var allowedExtensions = new[] { ".jpg", ".png" };
            var attribute = new AllowedExtensionsAttribute(allowedExtensions)
            {
                ErrorMessage = "Invalid file type."
            };

            var files = new Mock<IFormFileCollection>();
            var fileList = new List<IFormFile>
            {
                CreateMockFile("image1.jpg"),
                CreateMockFile("document.pdf") // Invalid extension
            };
            files.Setup(f => f.GetEnumerator()).Returns(fileList.GetEnumerator());

            // Act
            var result = attribute.GetValidationResult(files.Object, new ValidationContext(files.Object));

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Invalid file type.", result?.ErrorMessage);
        }

        private IFormFile CreateMockFile(string fileName)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns(fileName);
            return fileMock.Object;
        }
    }
}

