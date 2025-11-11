using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class AmountPerUomCalculator : IRebateIncentiveCalculator
    {
        public IncentiveType IncentiveType => IncentiveType.AmountPerUom;

        public bool IsValid(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return (product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) && rebate.Amount != 0 && request.Volume != 0);
        }

        public decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            decimal rebateAmount = 0.00m;
            if (IsValid(request, rebate, product))
            {
                rebateAmount = rebate.Amount * request.Volume;
            }
            return rebateAmount;
        }
    }
}
