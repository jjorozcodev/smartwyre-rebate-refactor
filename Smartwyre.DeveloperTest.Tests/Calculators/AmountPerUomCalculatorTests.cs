using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators
{
    public class AmountPerUomCalculatorTests
    {
        private readonly AmountPerUomCalculator _calculator = new AmountPerUomCalculator();
        private readonly Product _defaultProduct = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllConditionsMet()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 5 };
            var rebate = new Rebate { Amount = 1.00m };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, _defaultProduct);

            // Assert
            Assert.True(validatedInputs.IsValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenRequestVolumeIsZero()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 0 };
            var rebate = new Rebate { Amount = 1.00m };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, _defaultProduct);

            // Assert
            Assert.False(validatedInputs.IsValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenRebateAmountIsZero()
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = 5 };
            var rebate = new Rebate { Amount = 0.00m };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, _defaultProduct);

            // Assert
            Assert.False(validatedInputs.IsValid);
        }

        [Theory]
        [InlineData(2.50, 10, 25.00)]
        [InlineData(0.50, 4, 2.00)]
        public void Calculate_ShouldCalculateCorrectAmount(decimal amount, int volume, decimal expectedAmount)
        {
            // Arrange
            var request = new CalculateRebateRequest { Volume = volume };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
            var rebate = new Rebate { Amount = amount };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, product);
            var result = _calculator.Calculate(validatedInputs);

            // Assert
            Assert.Equal(expectedAmount, result.RebateAmount);
        }
    }
}
