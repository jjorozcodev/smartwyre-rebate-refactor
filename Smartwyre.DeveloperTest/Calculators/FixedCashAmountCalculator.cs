using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class FixedCashAmountCalculator : IRebateIncentiveCalculator
    {
        public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

        public bool IsValid(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return (product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) && rebate.Amount != 0);
        }

        public decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            decimal rebateAmount = 0.00m;
            if (IsValid(request, rebate, product))
            {
                rebateAmount = rebate.Amount;
            }
            return rebateAmount;
        }
    }
}
