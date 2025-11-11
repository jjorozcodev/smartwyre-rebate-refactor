using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class FixedCashAmountCalculator : IRebateIncentiveCalculator
    {
        public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

        public ValidatedRebateData ValidateInputs(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
                return ValidatedRebateData.Failure("Invalid incentive type.");
            if (rebate.Amount == 0)
                return ValidatedRebateData.Failure("Rebate amount cannot be zero.");

            return ValidatedRebateData.Success(
                    this.IncentiveType,
                    product.Price,
                    rebate.Amount,
                    rebate.Percentage,
                    request.Volume
                );
        }

        public decimal Calculate(ValidatedRebateData validatedData)
        {
            if (!validatedData.IsValid)
                throw new ArgumentException("Invalid input data.");
            if (validatedData.IncentiveTypeValidated != this.IncentiveType)
                throw new ArgumentException("Validated data is incorrect for this calculation operation.");

            return validatedData.RebateAmount;
        }
    }
}
