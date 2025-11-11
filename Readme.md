# Smartwyre Rebate Service Refactoring
**************************************
Developer Test Instructions: https://github.com/Smartwyre/developer-interview-test

## Engineering Principle: Strategy Pattern, Result Pattern, and TDD

This project involves a strategic refactoring of the RebateService to address violations of the Open/Closed Principle (OCP) and the Single Responsibility Principle (SRP), transforming a large conditional block (switch) into a modular design based on the Strategy Pattern.

**1. The Challenge & Safety Net (Golden Master)**

The original Calculate method contained a large switch statement tightly coupled to the IncentiveType, mandating modifications to the service every time a new incentive type was introduced (OCP violation).

Before Refactoring: Characterization Testing (Golden Master)
Crucially, before attempting any refactoring or modification of the core business logic in RebateService, Characterization Tests (Golden Master) were implemented using Verify.Xunit. This step served as an essential safety net, capturing and locking down the existing behavior of the legacy code. This guarantees that the refactoring preserved 100% of the existing business logic, including specific calculations and failure rules.

**2. The Solution Applied: Decoupling and Pure Functions**

The refactoring was executed in atomic commits, focused on decoupling and modularity. This structure ensures Adherence to SOLID principles, High Extensibility, and Improved Testability.

A. Strategy Pattern (OCP & SRP)

Interface: IRebateIncentiveCalculator was created to define the contract.

Concrete Strategies: FixedCashAmountCalculator, FixedRateRebateCalculator, and AmountPerUomCalculator were created, each handling its own specific validation and calculation logic (fulfilling SRP).

B. Strategy Resolution and IoC Integration

Resolver (Service Locator): An IRebateCalculatorResolver was implemented to resolve the correct strategy based on the IncentiveType of the Rebate, centralizing the dependency lookup via Inversion of Control (IoC).

Null Object: The Null Object Pattern (NullRebateCalculator) was utilized to ensure that the RebateService doesn't need to check for null strategies, simplifying the Calculate method and gracefully handling unimplemented incentive types.

C. Data Pipeline and Error Management (Result/Result Object Patterns)

To move towards a more functional programming style and eliminate exceptions from the core flow, a two-stage Result pattern was implemented, enforcing a sequential data pipeline:

Input Validation (Result Object Pattern): The validateInputs method returns a Result Object that, on success, explicitly carries a clean data structure (e.g., ValidatedRebateData). This pattern guarantees that the calculation step is only ever executed with validated, structured input, securing the sequential flow.

Calculation Execution (Result Pattern): The primary Calculate method returns the CalculationResult structure. This immutable object explicitly carries either the final calculated Value (on successful calculation) or a detailed ErrorMessage (on failure). This result is then consistently returned and managed by the higher-level RebateService.

Guard Clauses: Calculator methods now rely heavily on Guard Clauses at the start of the ValidateInputs method. Instead of throwing exceptions for invalid input (e.g., negative amounts), these clauses immediately return ValidatedRebateData.Failure("...").

Clarity and Flow: This dual-pattern approach eliminates the need for try/catch blocks in the core business logic, making the code's success and failure paths explicit and highly testable.

D. Unit Tests & Quality Assurance

Coverage includes all Concrete Strategies (FixedCashAmountCalculator, FixedRateRebateCalculator, AmountPerUomCalculator), the Resolver, and the Null Object fallback.

Tests now explicitly validate the new Result Pattern by asserting the Success property and checking the ErrorMessage content for all known failure states (negative values, null input).

Confirms 100% code coverage across all new classes, guaranteeing code robustness and preventing future regressions.

<img width="927" height="459" alt="image" src="https://github.com/user-attachments/assets/e8c86fd1-6fcf-43db-934e-4c2b36e53c64" />




**3. Conclusion & Engineering Excellence**

The RebateService is now Open for Extension (adding a new incentive only requires creating a new class implementing the interface) and Closed for Modification (the core RebateService remains untouched).
The implementation of the Result Pattern further enhances reliability by transforming runtime exceptions into explicit, predictable data flows.

**4. Next Steps & Pending Tasks (Completing Deliverables)**

| Task | Rationale & Status |
| ------------- | ------------- |
| Run Console Runner | Implement the logic within the Smartwyre.DeveloperTest.Runner application. This involves setting up the Dependency Injection (DI) container (e.g., using IServiceCollection), configuring the IRebateCalculatorResolver, and accepting user input to run the refactored RebateService. |
