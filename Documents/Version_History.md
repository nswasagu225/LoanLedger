# LoanLedger Version History

## Version 0.0.1

Status: Environment Setup

Completed:

- Project structure created
- GitHub repository created
- Development Journal created
- Learning Notes created
- Visual Studio Community installed

Next:

- .NET SDK
- Git Configuration
- PostgreSQL

## Version 0.0.1

Release Date: 04 August 2026

Completed

- Project structure
- Development documentation
- Git configuration
- Initial GitHub repository
- First project commit

Status

Environment setup completed.

## Version 0.0.2

Release Date:
06 August 2026

Completed

- Entity Framework Core integration
- PostgreSQL database connection
- Initial database migration
- User entity
- User repository
- Password hashing
- Registration API
- Login API
- JWT token service
- Authentication architecture
- OpenAPI endpoints

Status

Authentication foundation completed successfully.

## Version 0.12.0

### Authentication Foundation

- Authentication architecture completed.
- Password hashing implemented.
- JWT infrastructure implemented.
- Registration service implemented.
- Login infrastructure prepared.
- User Repository implemented.
- Dependency Injection configured.
- Application builds successfully.

Version 0.0.2

Release Date:
07 August 2026

Completed

- Authentication module
- User registration
- Login
- JWT authentication
- Password hashing
- PostgreSQL integration
- Repository Pattern
- API testing

Status

Authentication foundation completed successfully.

---

## Version 0.0.2

Release Date:
07 August 2026

### Added

Authentication Module

- User Registration
- Login with Email
- Login with Phone Number
- Password Hashing
- JWT Token Authentication

Contacts Module

- Contact Management
- Create Contact API
- Get Contacts API
- Duplicate Contact Validation

Database

- PostgreSQL integration
- Entity Framework migrations
- User-Contact relationship

Testing

- Registration tested
- Login tested
- Contacts tested

Status

Backend foundation completed successfully.
## Version 0.0.4

Release Date: 07 August 2026

### Added

- Contact Management
- Loan Categories
- Category Repository
- Category Service
- Category API
- Contact API improvements

### Status

Core reference data modules completed.

## Version 0.0.4

Release Date:
07 August 2026

Completed

- JWT Authentication completed
- Contacts module completed
- Loan Categories module completed
- Duplicate validation
- PostgreSQL migrations
- API testing completed

Status

Authentication + Contacts + Categories stable.
Version 0.0.4

Release Date: 07 August 2026

Completed
- Contacts module
- Categories module
- Loan entity
- Loan database table
- Loan relationships

Status

Core financial data model established.
Version 0.0.5

Release Date: 07 August 2026

Completed
- Loan repository
- Loan DTOs
- Loan infrastructure

Status

Loan module backend started.

Version 0.0.6

Release Date: 07 August 2026

Completed
- Loan service
- Loan controller
- Loan API endpoints

Status

Loan module is now fully accessible through REST APIs.

## Version 0.0.7

**Release Date: 08 August 2026**

### Added

* Loan Transactions module
* Loan Transaction entity
* Loan Transaction types
* Payment methods
* Transaction repository
* Transaction service
* Transaction DTO
* Transaction controller
* Transaction creation API
* Transaction retrieval APIs
* Loan transaction history API
* Transaction deletion API
* Repayment validation
* Automatic loan balance updates
* Automatic transaction reversal
* Transaction type names in API responses
* Payment method names in API responses

### Validation

* Prevented invalid transaction amounts
* Prevented transactions against closed loans
* Prevented repayment above current loan balance
* Verified loan balance after repayment
* Verified balance restoration after transaction deletion

### Status

**Loan Transactions module completed and tested successfully.**

The backend now supports the fundamental financial transaction lifecycle required by LoanLedger.

## Version History – New Entry

### Version / Milestone: Sprint 19 – Transaction Accounting Enhancement

**Status:** Completed

### Completed Features

* Added `AdjustmentDirection`.
* Added `WaiverType`.
* Added waiver transaction support.
* Added adjustment increase/decrease support.
* Updated transaction creation logic.
* Updated transaction update logic.
* Updated transaction deletion/reversal logic.
* Centralized transaction balance effects.
* Added validation for adjustment direction.
* Added validation for waiver type.
* Added automatic loan closure when balance reaches zero.
* Added automatic loan reopening when appropriate.
* Created `AddAdjustmentAndWaiverSupport` EF Core migration.
* Successfully applied migration to PostgreSQL.
* Tested Adjustment Increase through API.
* Tested Adjustment Decrease through API.
* Tested Principal Waiver through API.
* Verified transaction history responses.
* Verified loan summary balance calculations.
* Successfully completed `dotnet build`.

### Verification Result

```text
Build succeeded.
```

The transaction accounting foundation is now ready for the reporting layer.

# Project Roadmap – Updated Status

## Completed

### Loan Transaction Accounting

* [x] Disbursement
* [x] Repayment
* [x] Interest
* [x] Penalty
* [x] Refund
* [x] Adjustment
* [x] Adjustment Increase
* [x] Adjustment Decrease
* [x] Waiver
* [x] Waiver Type
* [x] Transaction update/reversal
* [x] Transaction deletion/reversal
* [x] Automatic balance calculation
* [x] Automatic loan closure
* [x] Loan reopening after balance-changing transaction modification
* [x] Database migration
* [x] API verification
* [x] Backend build verification

## Next Major Stage

### Reporting & PDF Export

The next stage will introduce a reporting system capable of producing:

* [ ] Individual loan reports
* [ ] All-loan/user reports
* [ ] Weekly reports
* [ ] Monthly reports
* [ ] Custom date-range reports
* [ ] Opening balance
* [ ] Period transactions
* [ ] Closing balance
* [ ] Repayment summaries
* [ ] Interest summaries
* [ ] Penalty summaries
* [ ] Adjustment summaries
* [ ] Waiver summaries
* [ ] Refund summaries
* [ ] PDF generation
* [ ] Complete account export
* [ ] Historical record preservation for users leaving the platform

### Reporting Principle

The reporting system should use the same verified transaction accounting logic already implemented in the backend. Reports must distinguish between amounts actually paid and amounts forgiven or adjusted.

The final account export should allow a user who chooses to stop using LoanLedger to obtain a complete copy of their available loan records.



---

# 4. Version History — add the next milestone

Don't call this a released version yet because we haven't implemented it.

Add:

```markdown
# Upcoming Milestone – Workspace Architecture

## Planned Version

Version 0.0.8

## Planned Release

August 2026

## Objective

Introduce the Workspace architecture into LoanLedger.

## Planned Features

- Workspace entity
- Workspace type
- Individual workspace
- Business workspace
- Cooperative workspace
- Organization workspace
- Workspace ownership
- Workspace repository
- Workspace service
- Workspace controller
- Workspace-aware authorization
- Workspace data isolation

## Architecture

User
↓
Workspace
↓
Financial Records

## Important Principle

Workspace Type and Loan/Agreement Type remain separate concepts.

## Status

Planned

The existing authentication, contacts, categories, loans, transactions, transaction accounting, and loan summary modules remain operational.