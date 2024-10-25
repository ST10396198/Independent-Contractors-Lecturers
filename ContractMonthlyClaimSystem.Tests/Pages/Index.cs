using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using ContractMonthlyClaimSystem.Pages;

public class IndexModelTests
{
    [Fact]
    public void IndexModel_OnGet_SetsExpectedValues()
    {
        // Arrange
        var logger = new NullLogger<IndexModel>(); // Use a null logger for testing
        var pageModel = new IndexModel(logger);

        // Act
        pageModel.OnGet();

        // Assert
        Assert.NotNull(pageModel);
    }
}

