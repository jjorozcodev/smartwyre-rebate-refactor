using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators
{
    public class FixedRateRebateCalculatorTests
    {
        private readonly FixedRateRebateCalculator _calculator = new FixedRateRebateCalculator();
        private readonly Product _defaultProduct = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100 };

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllConditionsMet()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 10 };
            var rebate = new Rebate { Percentage = 0.05m };

            // Act
            var isValid = _calculator.IsValid(request, rebate, _defaultProduct);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenRebatePercentageIsZero()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 10 };
            var rebate = new Rebate { Percentage = 0.00m };

            // Act
            var isValid = _calculator.IsValid(request, rebate, _defaultProduct);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenRequestVolumeIsZero()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 0 };
            var rebate = new Rebate { Percentage = 0.05m };

            // Act
            var isValid = _calculator.IsValid(request, rebate, _defaultProduct);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(10.00, 0.10, 5, 5.00)]
        [InlineData(50.00, 0.02, 20, 20.00)]
        public void Calculate_ShouldCalculateCorrectAmount(decimal price, decimal percentage, int volume, decimal expectedAmount)
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = volume };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = price };
            var rebate = new Rebate { Percentage = percentage };
            // Act
            var calculatedAmount = _calculator.Calculate(request, rebate, product);

            // Assert
            Assert.Equal(expectedAmount, calculatedAmount);
        }
    }
}
