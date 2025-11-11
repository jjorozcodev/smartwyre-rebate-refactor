# Smartwyre Rebate Service Refactoring
**************************************
Developer Test Instructions: https://github.com/Smartwyre/developer-interview-test

## Engineering Principle: Strategy Pattern and TDD

This project involves a strategic refactoring of the RebateService to address violations of the Open/Closed Principle (OCP) and the Single Responsibility Principle (SRP), transforming a large conditional block (switch) into a modular design based on the Strategy Pattern.

**1. The Challenge & Safety Net**

The original Calculate method contained a large switch statement tightly coupled to the IncentiveType, mandating modifications to the service every time a new incentive type was introduced (OCP violation).

Before Refactoring: Characterization Testing (Golden Master)
Crucially, before attempting any refactoring or modification of the core business logic in RebateService, Characterization Tests (Golden Master) were implemented using Verify.Xunit. This step served as an essential safety net, capturing and locking down the existing behavior of the legacy code. This guarantees that the refactoring preserved 100% of the existing business logic, including specific calculations and failure rules.

**2. The Solution Applied: Decoupling via Strategy**

The refactoring was executed in atomic commits, focused on decoupling and modularity. This structure ensures Adherence to SOLID principles, High Extensibility, and Improved Testability.

A. Strategy Pattern (OCP & SRP)

Interface: IRebateIncentiveCalculator was created to define the contract (IsValid and Calculate).

Concrete Strategies: FixedCashAmountCalculator, FixedRateRebateCalculator, and AmountPerUomCalculator were created, each handling its own specific validation and calculation logic (fulfilling SRP).

B. Service Locator and Null Object (IoC Integration)

Resolver (Service Locator): An IRebateCalculatorResolver was implemented to resolve the correct strategy based on the IncentiveType of the Rebate, centralizing the dependency lookup.

Null Object: The Null Object Pattern (NullRebateCalculator) was utilized to ensure that the RebateService doesn't need to check for null strategies, simplifying the Calculate method and gracefully handling unimplemented incentive types.

**3. Conclusion & Engineering Excellence**

The RebateService is now Open for Extension (adding a new incentive only requires creating a new class implementing the interface) and Closed for Modification (the core RebateService remains untouched).

**4. Next Steps & Pending Tasks (Completing Deliverables)**

| Task | Rationale & Status |
| ------------- | ------------- |
| Isolate Unit Tests for Calculators | Create dedicated Unit Tests for each concrete calculator strategy (FixedCashAmountCalculator, etc.) in the Smartwyre.DeveloperTest.Tests project to test their logic in isolation, separate from the Characterization Tests. |
| Run Console Runner | Implement the logic within the Smartwyre.DeveloperTest.Runner application. This involves setting up the Dependency Injection (DI) container (e.g., using IServiceCollection), configuring the IRebateCalculatorResolver, and accepting user input to run the refactored RebateService. |
| Null Object Integration | Ensure the DI container correctly maps all strategies and registers the NullRebateCalculator for scenarios where the IncentiveType is unknown or unsupported, solidifying the Strategy implementation. |
