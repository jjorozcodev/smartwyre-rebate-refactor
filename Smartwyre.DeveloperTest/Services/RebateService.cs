using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IEnumerable<IRebateIncentiveCalculator> _calculators;

    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;

    public RebateService(
        IEnumerable<IRebateIncentiveCalculator> calculators,
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore)
    {
        _calculators = calculators;

        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
    }

    private bool TryGetValidEntities(
        CalculateRebateRequest request,
        out Rebate rebate,
        out Product product)
    {
        rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate == null || product == null)
        {
            return false;
        }

        return true;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult() { Success = false };

        if (!TryGetValidEntities(request, out var rebate, out var product))
        {
            return result;
        }

        IRebateIncentiveCalculator incentiveCalculator = _calculators.FirstOrDefault(c => c.IncentiveType == rebate.Incentive);

        if (incentiveCalculator is null) return result;

        var rebateAmount = 0m;
        if(incentiveCalculator.IsValid(request, rebate, product))
        {
            result.Success = true;
            rebateAmount = incentiveCalculator.Calculate(request, rebate, product);

            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        }

        return result;
    }
}
