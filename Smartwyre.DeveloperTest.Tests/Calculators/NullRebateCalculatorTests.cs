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
            var validatedInputs = _calculator.ValidateInputs(request, rebate, product);

            // Assert
            Assert.False(validatedInputs.IsValid);
        }

        [Fact]
        public void Calculate_ShouldAlwaysReturnZero()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 100 };
            var rebate = new Rebate { Amount = 999m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, product);
            var result = _calculator.Calculate(validatedInputs);

            // Assert
            Assert.Equal(0.00m, result.RebateAmount);
        }

        [Fact]
        public void IncentiveType_ShouldBeNone()
        {
            // Assert
            Assert.Equal(-1, (int)_calculator.IncentiveType);
        }
    }
}
