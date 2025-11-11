using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class FixedRateRebateCalculator : IRebateIncentiveCalculator
    {
        public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

        public ValidatedRebateData ValidateInputs(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
                return ValidatedRebateData.Failure("Invalid incentive type.");
            if (rebate.Percentage == 0)
                return ValidatedRebateData.Failure("Rebate percentage cannot be zero.");
            if (product.Price == 0)
                return ValidatedRebateData.Failure("Product price cannot be zero.");
            if (request.Volume == 0)
                return ValidatedRebateData.Failure("Request volume cannot be zero.");

            return ValidatedRebateData.Success(
                    this.IncentiveType,
                    product.Price,
                    rebate.Amount,
                    rebate.Percentage,
                    request.Volume
                );
        }

        public RebateCalculationResult Calculate(ValidatedRebateData validatedData)
        {
            if (!validatedData.IsValid)
                return RebateCalculationResult.Failure("Invalid input data.");
            if (validatedData.IncentiveTypeValidated != this.IncentiveType)
                return RebateCalculationResult.Failure("Validated data is incorrect for this calculation operation.");

            decimal calculated = validatedData.ProductPrice * validatedData.RebatePercentage * validatedData.RequestVolume;
            return RebateCalculationResult.Successful(calculated);
        }
    }
}
