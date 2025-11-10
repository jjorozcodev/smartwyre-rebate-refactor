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
                rebateAmount = CalculateFixedCashAmount(result, rebate, product, rebateAmount);
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
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
        {
            result.Success = false;
        }
        else if (rebate.Amount == 0 || request.Volume == 0)
        {
            result.Success = false;
        }
        else
        {
            rebateAmount += rebate.Amount * request.Volume;
            result.Success = true;
        }

        return rebateAmount;
    }

    private static decimal CalculateFixedRateRebate(CalculateRebateRequest request, CalculateRebateResult result, Rebate rebate, Product product, decimal rebateAmount)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
        {
            result.Success = false;
        }
        else if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0)
        {
            result.Success = false;
        }
        else
        {
            rebateAmount += product.Price * rebate.Percentage * request.Volume;
            result.Success = true;
        }

        return rebateAmount;
    }

    private static decimal CalculateFixedCashAmount(CalculateRebateResult result, Rebate rebate, Product product, decimal rebateAmount)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
        {
            result.Success = false;
        }
        else if (rebate.Amount == 0)
        {
            result.Success = false;
        }
        else
        {
            rebateAmount = rebate.Amount;
            result.Success = true;
        }

        return rebateAmount;
    }
}
