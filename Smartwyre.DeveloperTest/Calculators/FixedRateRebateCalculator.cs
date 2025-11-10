using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class FixedRateRebateCalculator
    {
        public bool IsValid(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return (product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) && rebate.Percentage != 0 && product.Price != 0 && request.Volume != 0);
        }

        public decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            decimal rebateAmount = 0.00m;
            if (IsValid(request, rebate, product))
            {
                rebateAmount = product.Price * rebate.Percentage * request.Volume;
            }
            return rebateAmount;
        }
    }
}
