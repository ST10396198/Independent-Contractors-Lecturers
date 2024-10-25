using Xunit;
using ContractMonthlyClaimSystem.Models.Enums;

public class StatusTests
{
    [Fact]
    public void Enum_ShouldHaveCorrectValues()
    {
        // Assert that the enum values are as expected
        Assert.Equal(0, (int)Status.Pending);
        Assert.Equal(1, (int)Status.Approved);
        Assert.Equal(2, (int)Status.Rejected);
    }

    [Theory]
    [InlineData(Status.Pending)]
    [InlineData(Status.Approved)]
    [InlineData(Status.Rejected)]
    public void Enum_ShouldHaveDefinedValues(Status status)
    {
        // This test simply checks that the enum values can be passed correctly
        Assert.IsAssignableFrom<Status>(status);
    }

    [Fact]
    public void Enum_Count_ShouldBeThree()
    {
        // Assert that the number of enum values is as expected
        Assert.Equal(3, Enum.GetValues(typeof(Status)).Length);
    }
}

