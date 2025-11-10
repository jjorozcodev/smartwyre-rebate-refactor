using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
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
        var result = new CalculateRebateResult();

        if (!TryGetValidEntities(request, out var rebate, out var product))
        {
            result.Success = false;
            return result;
        }

        var rebateAmount = 0m;

        switch (rebate.Incentive)
        {
            case IncentiveType.FixedCashAmount:
                rebateAmount = CalculateFixedCashAmount(request, result, rebate, product, rebateAmount);
                break;

            case IncentiveType.FixedRateRebate:
                rebateAmount = CalculateFixedRateRebate(request, result, rebate, product, rebateAmount);
                break;

            case IncentiveType.AmountPerUom:
                rebateAmount = CalculateAmountPerUom(request, result, rebate, product, rebateAmount);
                break;
            default:
                break;
        }

        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        }

        return result;
    }

    private static decimal CalculateAmountPerUom(CalculateRebateRequest request, CalculateRebateResult result, Rebate rebate, Product product, decimal rebateAmount)
    {
        result.Success = false;

        var calcAmountPerUom = new AmountPerUomCalculator();

        if (calcAmountPerUom.IsValid(request, rebate, product))
        {
            rebateAmount = calcAmountPerUom.Calculate(request, rebate, product);
            result.Success = true;
        }

        return rebateAmount;
    }

    private static decimal CalculateFixedRateRebate(CalculateRebateRequest request, CalculateRebateResult result, Rebate rebate, Product product, decimal rebateAmount)
    {
        result.Success = false;

        var calcFixedRateRebate = new FixedRateRebateCalculator();

        if (calcFixedRateRebate.IsValid(request, rebate, product))
        {
            rebateAmount = calcFixedRateRebate.Calculate(request, rebate, product);
            result.Success = true;
        }

        return rebateAmount;
    }

    private static decimal CalculateFixedCashAmount(CalculateRebateRequest request, CalculateRebateResult result, Rebate rebate, Product product, decimal rebateAmount)
    {
        result.Success = false;

        var calcFixedCashAmount = new FixedCashAmountCalculator();

        if (calcFixedCashAmount.IsValid(request, rebate, product))
        {
            rebateAmount = calcFixedCashAmount.Calculate(request, rebate, product);
            result.Success = true;
        }

        return rebateAmount;
    }
}
