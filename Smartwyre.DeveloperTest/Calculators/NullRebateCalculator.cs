using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class NullRebateCalculator : IRebateIncentiveCalculator
    {
        public IncentiveType IncentiveType => (IncentiveType)(-1);

        public decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return 0.0m;
        }

        public bool IsValid(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return false;
        }
    }
}
