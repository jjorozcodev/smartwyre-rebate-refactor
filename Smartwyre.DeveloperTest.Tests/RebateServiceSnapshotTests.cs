using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateServiceSnapshotTests
    {
        private readonly RebateService _service;
        private readonly Mock<IRebateDataStore> _rebateDataStoreMock;
        private readonly Mock<IProductDataStore> _productDataStoreMock;

        public RebateServiceSnapshotTests()
        {
            _rebateDataStoreMock = new Mock<IRebateDataStore>();
            _productDataStoreMock = new Mock<IProductDataStore>();

            _service = new RebateService(_rebateDataStoreMock.Object, _productDataStoreMock.Object);
        }

        [Fact]
        public void RebateService_Calculate_ShouldReturnSuccess_WhenIsFixedCashAmount()
        {
            CalculateRebateRequest request = new CalculateRebateRequest
            {
                ProductIdentifier = "PRD",
                RebateIdentifier = "RBT"
            };

            Product product = new() { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
            Rebate rebate = new() { Incentive = IncentiveType.FixedCashAmount, Amount = 100 };

            _rebateDataStoreMock.Setup(x => x.GetRebate(It.IsAny<string>())).Returns(rebate);
            _productDataStoreMock.Setup(x => x.GetProduct(It.IsAny<string>())).Returns(product);

            var result = _service.Calculate(request);

            Assert.True(result.Success);
        }
    }
}
