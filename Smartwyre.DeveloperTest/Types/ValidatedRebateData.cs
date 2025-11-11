namespace Smartwyre.DeveloperTest.Types
{
    public record ValidatedRebateData
    {
        public bool IsValid { get; init; } = false;
        public string ErrorMessage { get; init; } = string.Empty;

        public decimal ProductPrice { get; init; }
        public decimal RebateAmount { get; init; }
        public decimal RebatePercentage { get; init; }
        public decimal RequestVolume { get; init; }

        public IncentiveType IncentiveTypeValidated { get; init; }

        private ValidatedRebateData() { }

        public static ValidatedRebateData Success(
            IncentiveType type,
            decimal productPrice,
            decimal rebateAmount,
            decimal rebatePercentage,
            decimal requestVolume) => new()
            {
                IsValid = true,
                IncentiveTypeValidated = type,
                ProductPrice = productPrice,
                RebateAmount = rebateAmount,
                RebatePercentage = rebatePercentage,
                RequestVolume = requestVolume
            };

        public static ValidatedRebateData Failure(string message) => new()
        {
            IsValid = false,
            ErrorMessage = message
        };
    }
}
