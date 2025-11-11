using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators
{
    public interface IRebateIncentiveCalculator
    {
        IncentiveType IncentiveType { get; }
        ValidatedRebateData ValidateInputs(CalculateRebateRequest request, Rebate rebate, Product product);
        decimal Calculate(ValidatedRebateData validatedData);
    }
}