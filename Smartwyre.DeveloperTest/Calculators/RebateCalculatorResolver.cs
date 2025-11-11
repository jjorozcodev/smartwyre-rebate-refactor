using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class RebateCalculatorResolver : IRebateCalculatorResolver
    {
        private readonly IEnumerable<IRebateIncentiveCalculator> _calculators;

        private const int NullObjectValue = -1;

        public RebateCalculatorResolver(IEnumerable<IRebateIncentiveCalculator> calculators)
        {
            _calculators = calculators;
        }

        public IRebateIncentiveCalculator Resolve(IncentiveType incentiveType)
        {
            IRebateIncentiveCalculator calculator = _calculators
                .FirstOrDefault(c => c.IncentiveType == incentiveType);

            if (calculator != null) return calculator;

            IRebateIncentiveCalculator nullCalculator = _calculators
                .SingleOrDefault(c => (int)c.IncentiveType == NullObjectValue);

            return nullCalculator;
        }
    }
}
