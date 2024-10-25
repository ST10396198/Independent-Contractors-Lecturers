using Xunit;
using ContractMonthlyClaimSystem.Models.Enums;

namespace ContractMonthlyClaimSystem.Tests
{
    public class CourseTests
    {
        [Fact]
        public void Enum_ShouldHaveCorrectValues()
        {
            // Assert that the enum values are as expected
            Assert.Equal(0, (int)Course.Course1);
            Assert.Equal(1, (int)Course.Course2);
            Assert.Equal(2, (int)Course.Course3);
        }

        [Theory]
        [InlineData(Course.Course1)]
        [InlineData(Course.Course2)]
        [InlineData(Course.Course3)]
        public void Enum_ShouldHaveDefinedValues(Course course)
        {
            // This test checks that each enum value can be passed correctly
            Assert.IsAssignableFrom<Course>(course);
        }

        [Fact]
        public void Enum_Count_ShouldBeThree()
        {
            // Assert that the number of enum values is as expected
            Assert.Equal(3, Enum.GetValues(typeof(Course)).Length);
        }

        [Fact]
        public void Enum_ShouldHaveExpectedNames()
        {
            // Assert that the names of the enum values are as expected
            Assert.Equal("Course1", Course.Course1.ToString());
            Assert.Equal("Course2", Course.Course2.ToString());
            Assert.Equal("Course3", Course.Course3.ToString());
        }
    }
}

