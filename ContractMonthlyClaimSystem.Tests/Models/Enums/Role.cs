using Xunit;
using ContractMonthlyClaimSystem.Models.Enums;

namespace ContractMonthlyClaimSystem.Tests
{
    public class RoleTests
    {
        [Fact]
        public void Enum_ShouldHaveCorrectValues()
        {
            // Assert that the enum values are as expected
            Assert.Equal(0, (int)Role.Lecturer);
            Assert.Equal(1, (int)Role.Admin);
        }

        [Theory]
        [InlineData(Role.Lecturer)]
        [InlineData(Role.Admin)]
        public void Enum_ShouldHaveDefinedValues(Role role)
        {
            // This test checks that each enum value can be passed correctly
            Assert.IsAssignableFrom<Role>(role);
        }

        [Fact]
        public void Enum_Count_ShouldBeTwo()
        {
            // Assert that the number of enum values is as expected
            Assert.Equal(2, Enum.GetValues(typeof(Role)).Length);
        }

        [Fact]
        public void Enum_ShouldHaveExpectedNames()
        {
            // Assert that the names of the enum values are as expected
            Assert.Equal("Lecturer", Role.Lecturer.ToString());
            Assert.Equal("Admin", Role.Admin.ToString());
        }
    }
}

