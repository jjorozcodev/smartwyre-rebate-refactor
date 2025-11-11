using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators
{
    public class NullRebateCalculator : IRebateIncentiveCalculator
    {
        public IncentiveType IncentiveType => (IncentiveType)(-1);

        public ValidatedRebateData ValidateInputs(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return ValidatedRebateData.Failure("Unsupported incentive type.");
        }

        public decimal Calculate(ValidatedRebateData validatedData)
        {
            return 0.0m;
        }

        
    }
}
