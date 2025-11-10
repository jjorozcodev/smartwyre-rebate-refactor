using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators
{
    public interface IRebateIncentiveCalculator
    {
        decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product);
        bool IsValid(CalculateRebateRequest request, Rebate rebate, Product product);
    }
}