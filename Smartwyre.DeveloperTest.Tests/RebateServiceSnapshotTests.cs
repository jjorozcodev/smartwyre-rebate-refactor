using Moq;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateServiceSnapshotTests
    {
        private static readonly List<object> _allResults = [];

        private readonly RebateService _service;
        private readonly Mock<IRebateDataStore> _rebateDataStoreMock;
        private readonly Mock<IProductDataStore> _productDataStoreMock;

        private static readonly IEnumerable<IRebateIncentiveCalculator> _rebateCalculators = new List<IRebateIncentiveCalculator>
        {
            new FixedCashAmountCalculator(),
            new FixedRateRebateCalculator(),
            new AmountPerUomCalculator()
        };

        public RebateServiceSnapshotTests()
        {
            _rebateDataStoreMock = new Mock<IRebateDataStore>();
            _productDataStoreMock = new Mock<IProductDataStore>();

            _service = new RebateService(
                        _rebateCalculators,
                        _rebateDataStoreMock.Object,
                        _productDataStoreMock.Object);
        }

        public static IEnumerable<object[]> GetSnapshotScenarios()
        {
            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-00" },
                null,
                null
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-01" },
                new Rebate { Incentive = IncentiveType.FixedCashAmount },
                new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-02" },
                new Rebate { Incentive = IncentiveType.FixedCashAmount },
                new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-03" },
                new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 50 },
                new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-11" },
                new Rebate { Incentive = IncentiveType.FixedRateRebate },
                null
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-12" },
                new Rebate { Incentive = IncentiveType.FixedRateRebate },
                new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-13", Volume = 0 },
                new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0 },
                new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 0 }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-14", Volume = 20 },
                new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.10m },
                new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 50 }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-21" },
                new Rebate { Incentive = IncentiveType.AmountPerUom },
                null
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-22" },
                new Rebate { Incentive = IncentiveType.AmountPerUom },
                new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-23", Volume = 0 },
                new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 0 },
                new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom }
            };

            yield return new object[] {
                new CalculateRebateRequest { RebateIdentifier = "R-24", Volume = 25 },
                new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 100 },
                new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom }
            };
        }

        [Theory]
        [MemberData(nameof(GetSnapshotScenarios))]
        public void RebateService_Calculate_ExecuteCases(
        CalculateRebateRequest request,
        Rebate rebate,
        Product product)
        {
            _rebateDataStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
            _productDataStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(product);

            decimal capturedAmount = 0m;
            _rebateDataStoreMock
                .Setup(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()))
                .Callback<Rebate, decimal>((r, amount) => capturedAmount = amount);

            var result = _service.Calculate(request);

            _allResults.Add(new
            {
                Success = result,
                CapturedAmount = capturedAmount.ToString("F2"),
                Request = request,
                ProductInfo = product,
                RebateInfo = rebate
            });
        }

        [Fact]
        public Task VerifyAllCollectedResults()
        {
            return Verifier.Verify(_allResults)
                .UseMethodName(this.GetType().Name);
        }
    }
}
