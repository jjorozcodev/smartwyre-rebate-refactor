namespace Smartwyre.DeveloperTest.Types
{
    public record RebateCalculationResult
    {
        public bool Success { get; init; }

        public decimal RebateAmount { get; init; }

        public string Message { get; init; } = string.Empty;

        private RebateCalculationResult() { }

        public static RebateCalculationResult Successful(decimal amount) => new()
        {
            Success = true,
            RebateAmount = amount,
            Message = "Rebate calculated successfully."
        };

        public static RebateCalculationResult Failure(string message) => new()
        {
            Success = false,
            RebateAmount = 0.00m,
            Message = message
        };
    }
}
