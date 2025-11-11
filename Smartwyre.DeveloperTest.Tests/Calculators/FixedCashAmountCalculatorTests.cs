using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators
{
    public class FixedCashAmountCalculatorTests
    {
        private readonly FixedCashAmountCalculator _calculator = new FixedCashAmountCalculator();
        private readonly Product _defaultProduct = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenProductSupportsIncentiveAndRebateAmountIsValid()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var rebate = new Rebate { Amount = 100.00m };

            // Act
            var isValid = _calculator.IsValid(request, rebate, _defaultProduct);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenProductDoesNotSupportIncentive()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var invalidProduct = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
            var rebate = new Rebate { Amount = 100.00m };

            // Act
            var isValid = _calculator.IsValid(request, rebate, invalidProduct);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenRebateAmountIsZero()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var rebate = new Rebate { Amount = 0.00m };

            // Act
            var isValid = _calculator.IsValid(request, rebate, _defaultProduct);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Calculate_ShouldReturnFixedAmount_WhenValid()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var expectedAmount = 75.50m;
            var rebate = new Rebate { Amount = expectedAmount };

            // Act
            var calculatedAmount = _calculator.Calculate(request, rebate, _defaultProduct);

            // Assert
            Assert.Equal(expectedAmount, calculatedAmount);
        }
    }
}
