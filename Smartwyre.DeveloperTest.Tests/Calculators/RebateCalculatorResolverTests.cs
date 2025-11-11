using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators
{
    public class RebateCalculatorResolverTests
    {
        private readonly IRebateCalculatorResolver _calculatorResolver;
        private static readonly IEnumerable<IRebateIncentiveCalculator> _rebateCalculators = new List<IRebateIncentiveCalculator>
        {
            new FixedCashAmountCalculator(),
            new FixedRateRebateCalculator(),
            new NullRebateCalculator() // Null Object Pattern
        };

        public RebateCalculatorResolverTests()
        {
            _calculatorResolver = new RebateCalculatorResolver(_rebateCalculators);
        }

        [Fact]
        public void Resolve_ShouldReturnFixedCashAmountCalculator_WhenRequested()
        {
            // Arrange & Act
            var result = _calculatorResolver.Resolve(IncentiveType.FixedCashAmount);

            // Assert
            Assert.IsType<FixedCashAmountCalculator>(result);
        }

        [Fact]
        public void Resolve_ShouldReturnFixedRateRebateCalculator_WhenRequested()
        {
            // Arrange & Act
            var result = _calculatorResolver.Resolve(IncentiveType.FixedRateRebate);

            // Assert
            Assert.IsType<FixedRateRebateCalculator>(result);
        }

        [Fact]
        public void Resolve_ShouldReturnNullCalculator_WhenIncentiveTypeDoesNotExist()
        {
            // Arrange & Act
            var result = _calculatorResolver.Resolve(IncentiveType.AmountPerUom);

            // Assert
            Assert.IsType<NullRebateCalculator>(result);
        }
    }
}
