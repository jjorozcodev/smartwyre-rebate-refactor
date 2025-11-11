using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators
{
    public class NullRebateCalculatorTests
    {
        private readonly NullRebateCalculator _calculator = new NullRebateCalculator();

        [Fact]
        public void IsValid_ShouldAlwaysReturnFalse()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 100 };
            var rebate = new Rebate { Amount = 999m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

            // Act
            var isValid = _calculator.IsValid(request, rebate, product);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Calculate_ShouldAlwaysReturnZero()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 100 };
            var rebate = new Rebate { Amount = 999m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

            // Act
            var calculatedAmount = _calculator.Calculate(request, rebate, product);

            // Assert
            Assert.Equal(0.00m, calculatedAmount);
        }

        [Fact]
        public void IncentiveType_ShouldBeNone()
        {
            // Assert
            Assert.Equal(-1, (int)_calculator.IncentiveType);
        }
    }
}
