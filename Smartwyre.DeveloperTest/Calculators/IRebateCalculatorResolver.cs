using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators
{
    public interface IRebateCalculatorResolver
    {
        IRebateIncentiveCalculator Resolve(IncentiveType incentiveType);
    }
}
