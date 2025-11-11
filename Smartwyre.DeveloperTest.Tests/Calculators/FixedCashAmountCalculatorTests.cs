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
            var validatedInputs = _calculator.ValidateInputs(request, rebate, _defaultProduct);

            // Assert
            Assert.True(validatedInputs.IsValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenProductDoesNotSupportIncentive()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var invalidProduct = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
            var rebate = new Rebate { Amount = 100.00m };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, invalidProduct);

            // Assert
            Assert.False(validatedInputs.IsValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenRebateAmountIsZero()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var rebate = new Rebate { Amount = 0.00m };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, _defaultProduct);

            // Assert
            Assert.False(validatedInputs.IsValid);
        }
        [Fact]
        public void Calculate_ShouldReturnInvalidResult_WhenInvalidInput()
        {
            // Arrange
            var validatedInput = ValidatedRebateData.Failure("Invalid input.");

            // Act
            var result = _calculator.Calculate(validatedInput);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnInvalidResult_WhenIncorrectIncentiveType()
        {
            // Arrange
            var validatedInput = ValidatedRebateData.Success(IncentiveType.AmountPerUom, 0.0m, 0.0m, 0.0m, 0.0m);

            // Act
            var result = _calculator.Calculate(validatedInput);

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public void Calculate_ShouldReturnFixedAmount_WhenValid()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var expectedAmount = 75.50m;
            var rebate = new Rebate { Amount = expectedAmount };

            // Act
            var validatedInputs = _calculator.ValidateInputs(request, rebate, _defaultProduct);
            var result = _calculator.Calculate(validatedInputs);

            // Assert
            Assert.Equal(expectedAmount, result.RebateAmount);
        }
    }
}
