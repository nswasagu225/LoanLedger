# LoanLedger Development Journal

## Project Information

Project Name: LoanLedger

Developer: Nasiru Sani

Project Start Date: 03 August 2026

Project Type:
Personal & Organizational Financial Relationship Management Platform

Technology Stack

- Flutter
- Dart
- ASP.NET Core
- C#
- PostgreSQL
- SQLite
- Git
- GitHub

---

## Sprint 1

### Objective

Prepare the complete development environment before writing any code.

### Tasks

- [x] Create project folder structure
- [x] Create GitHub Repository
- [ ] Create Learning Notes
- [ ] Install Visual Studio Community
- [ ] Install .NET SDK
- [ ] Install PostgreSQL
- [ ] Install Flutter
- [ ] Install Android Studio

---

## Notes

Today marks the official beginning of the LoanLedger project.

## Sprint 2 Completed

### Achievement

Successfully installed Visual Studio Community 2022.

### Result

Visual Studio launches successfully.

### Next Step

Install the .NET SDK and verify the development environment.

## Sprint 3

### Date

04 August 2026

### Objective

Verify the .NET development environment and learn the basic .NET CLI commands.

### Tasks Completed

- [x] Verified .NET SDK installation
- [x] Verified ASP.NET Core Runtime
- [x] Learned the purpose of the .NET SDK
- [x] Learned basic .NET CLI commands
- [x] Successfully ran:
  - dotnet --version
  - dotnet --info

### Results

Installed SDK Version:
9.0.316

Verified that the development environment is ready for ASP.NET Core development.

### Lessons Learned

- .NET is the development platform.
- C# is the programming language.
- ASP.NET Core is the web framework.
- Visual Studio is the Integrated Development Environment (IDE).
- The .NET CLI can be used to create, build, run, and publish applications.


## Sprint 4 Completed

### Achievement

Initialized Git repository.

Created first Git commit.

Connected local repository to GitHub.

Successfully pushed Version 0.0.1 to GitHub.

### Commit

b9d58a9 - Initial project structure and documentation

## Local Development Environment

Database Server:
PostgreSQL

Port:
5432

Database:
LoanLedgerDB

Username:
postgres

Password:
********

# Sprint 5 – System Analysis & Database Design

Duration:
Completed

Objectives

- Complete LoanLedger system analysis.
- Define functional and non-functional requirements.
- Design Entity Relationship Diagram (ERD).
- Design database tables.
- Define coding standards.
- Finalize Version 1.0 scope.

Completed Tasks

✔ Defined LoanLedger project vision.

✔ Designed application workflow.

✔ Finalized Contact Wallet.

✔ Designed Smart Agreement Wizard.

✔ Added support for:

- Cash Loans
- Credit Sales
- Agricultural Input Agreements
- Cooperative Loans
- Salary Advances
- Business Credit
- Multiple Currency
- Optional Interest
- Voice Notes
- Witnesses
- Attachments
- Offline Mode
- PDF & Image Reports

✔ Designed database entities.

✔ Designed database relationships.

✔ Defined coding standards.

✔ Planned project architecture using Clean Architecture.

Deliverables

- Requirements Analysis
- Database Design
- ERD
- Coding Standards
- Updated Roadmap

# Sprint 6 – Development Environment & Database Foundation

Status:
Completed

Objectives

- Install development tools.
- Install PostgreSQL.
- Install pgAdmin.
- Create LoanLedgerDB.
- Initialize SQL Script structure.

Completed Tasks

✔ PostgreSQL 18.4 installed.

✔ pgAdmin 4 installed.

✔ LoanLedgerDB created.

✔ SQL Script folder created.

✔ Initial SQL script created.

✔ Database documentation updated.

Deliverables

- PostgreSQL Environment
- LoanLedgerDB
- SQL Script Structure

# Sprint 7 – Database Design & Standards

Status:
Completed

Objectives

- Finalize database architecture.
- Design all Version 1.0 tables.
- Define relationships.
- Define database standards.
- Define naming conventions.
- Prepare SQL scripts.

Completed Tasks

✔ Finalized Users table design.

✔ Finalized Workspaces table design.

✔ Finalized Contacts table design.

✔ Finalized Agreements table design.

✔ Finalized Agreement Items table design.

✔ Finalized Repayments table design.

✔ Designed lookup tables.

✔ Designed Entity Relationship Diagram (ERD).

✔ Defined audit fields.

✔ Defined UUID strategy.

✔ Defined naming conventions.

✔ Defined indexing strategy.

✔ Defined security standards.

✔ Prepared SQL script numbering.

Deliverables

- Database Standards
- ERD
- SQL Script Plan
- Naming Standards

# Sprint 8 – Backend Foundation

Status:
In Progress

Objectives

- Create ASP.NET Core Solution
- Implement Clean Architecture
- Configure PostgreSQL
- Configure Entity Framework Core
- Create Users Entity
- Create Initial Migration

✔ Created LoanLedger solution.

✔ Added API project.

✔ Added Domain project.

✔ Added Application project.

✔ Added Infrastructure project.

✔ Added Test project.

✔ Connected project references.

✔ Successfully built the complete solution.
✔ Installed Microsoft.EntityFrameworkCore 9.0.8

✔ Installed Microsoft.EntityFrameworkCore.Design 9.0.8

✔ Installed Microsoft.EntityFrameworkCore.Tools 9.0.8

✔ Installed Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4

✔ Prepared Infrastructure for Entity Framework Core.

✔ Created BaseEntity.

✔ Created AuditableEntity.

✔ Created User entity.

✔ Created LoanLedgerDbContext.

✔ Connected User entity to Entity Framework.

✔ Configured PostgreSQL connection string.

# Sprint 9 – Entity Framework & First Migration

## Objectives
- Configure Entity Framework Core with PostgreSQL.
- Generate the first migration.
- Prepare LoanLedger for database creation.

## Completed
- Verified Infrastructure project configuration.
- Connected API to Infrastructure.
- Configured LoanLedgerDbContext.
- Prepared for first migration.
## Completed

✔ Generated InitialCreate migration.

✔ Entity Framework generated database schema successfully.

✔ Prepared LoanLedger database for deployment.
✔ Applied InitialCreate migration to PostgreSQL.

✔ Database tables created successfully.

# Sprint 10 – First Database Created

## Completed

✔ Generated InitialCreate migration.

✔ Applied migration to PostgreSQL.

✔ Created Users table.

✔ Created __EFMigrationsHistory table.

✔ Verified Entity Framework Code-First workflow.

## Notes

The project successfully transitioned from code-only to a working PostgreSQL-backed application.

# Sprint 11 – Domain Expansion

## Objectives

- Expand User entity.
- Prepare Authentication.
- Design Workspace model.
- Design Contact model.
- Improve Dashboard architecture.

## Notes

Version 1 now moves from infrastructure into real business functionality.
# Sprint 12 – Authentication Foundation

## Objectives

- Organize authentication folders.
- Design authentication architecture.
- Prepare JWT authentication.
- Implement password hashing.
- Design user registration workflow.
- Prepare login workflow.
- Prepare user repository.
- Prepare token generation.

## Completed

- Created Authentication folder structure.
- Created DTOs for Registration and Login.
- Created IAuthService.
- Created IJwtTokenService.
- Created IPasswordService.
- Implemented PasswordService.
- Implemented JwtTokenService.
- Implemented AuthService.
- Implemented UserRepository.
- Registered services using Dependency Injection.
- Configured JWT Settings.
- Prepared Login and Registration endpoints.
- Verified project builds successfully.

## Result

LoanLedger now has a complete Authentication Foundation ready for secure user authentication and authorization.

## Completed

✓ Authentication architecture

✓ Registration API

✓ Login API

✓ Password hashing service

✓ JWT token service

✓ User repository

✓ Dependency Injection

✓ Authentication controller

✓ OpenAPI integration

✓ PostgreSQL user persistence

✓ Login by Email or Phone support

Date: 07 August 2026

Sprint 12 Completed

Completed:
- Password hashing
- JWT authentication
- User registration
- Login using email
- Login using phone number
- Duplicate email validation
- Last login tracking
- API testing using Postman

Result:
Authentication module is fully operational.

# Sprint 13 – Contacts Module

## Objectives

- Design Contact entity
- Implement Contact Repository
- Implement Contact Service
- Implement Contact API
- Connect Contacts to Users
- Prevent duplicate contacts
- Test Contacts API

---

## Completed

### Domain
- Contact entity created
- User → Contacts relationship added

### Infrastructure
- ContactRepository implemented
- Entity Framework configuration completed
- PostgreSQL migration created
- Database updated successfully

### Application
- ContactService implemented
- Create Contact logic completed
- Get Contacts logic completed
- Duplicate contact validation added

### API
- ContactsController created
- POST /api/Contacts
- GET /api/Contacts/{userId}

### Testing
- Contact creation tested successfully
- Contact retrieval tested successfully
- Duplicate contact validation verified

---

## Result

The Contacts module is production-ready and integrated with the Authentication module.

Status:
✅ Completed

# Sprint 14 – Loan Categories

## Objectives

- Design LoanCategory entity
- Connect LoanCategory to User
- Configure Entity Framework relationship
- Prepare Category DTOs
- Prepare Category Service interface

---

## Completed

### Domain
- LoanCategory entity created
- User → LoanCategories relationship added

### Infrastructure
- DbSet<LoanCategory> added
- Entity Framework relationship configured

### Application
- CreateCategoryRequest
- CreateCategoryResponse
- CategoryDto
- ICategoryService

---

Status

🟡 In Progress
## Completed

- Category Repository
- Category Service
- Categories Controller
- Create Category API
- Get Categories API

Status

🟡 Ready for Testing
# Sprint 14 – Loan Categories

## Objectives

- Category Entity
- Category Repository
- Category Service
- Category API
- Category Testing

## Completed

- Created LoanCategory entity
- Repository implemented
- Service implemented
- REST API implemented
- Duplicate category validation
- User-specific category retrieval

Status

🟢 Completed

# Sprint 15 – Loan Categories

## Objectives

- Create Category entity
- Create Category repository
- Create Category service
- Create Category controller
- Prevent duplicate category names
- Test Category APIs

## Completed

✔ Category entity
✔ Category repository
✔ Category service
✔ Category controller
✔ Create Category API
✔ Get Categories API
✔ Duplicate validation
✔ EF Migration
✔ PostgreSQL update
✔ Postman testing

Status

Completed successfully.
Sprint 15 – Loan Accounts Foundation

Completed
- Loan entity created
- Loan relationships established
- EF Core migration added
- Loan table created

Sprint 16 – Loan Accounts

Completed
- Loan DTOs
- Loan repository
- Loan repository interface
- Repository registration

Sprint 17 – Loan Service

Completed
- Loan service
- Loan controller
- Loan creation endpoint
- Loan retrieval endpoint


# Sprint 18 – Loan Transactions

## Objectives

* Create the `LoanTransaction` entity
* Create transaction type enum
* Create payment method enum
* Establish Loan → LoanTransaction relationship
* Create transaction repository
* Create transaction repository interface
* Create transaction service
* Create transaction response DTO
* Create transaction controller
* Implement transaction creation
* Implement repayment balance validation
* Implement disbursement and other balance adjustments
* Implement transaction retrieval APIs
* Implement loan transaction history API
* Implement transaction deletion
* Automatically reverse the loan balance when a transaction is deleted
* Improve API responses with transaction type and payment method names
* Test transaction APIs
* Verify loan balance after transaction operations

## Completed

✔ `LoanTransaction` entity created

✔ `LoanTransactionType` enum created

✔ `PaymentMethod` enum created

✔ `LoanTransaction` → `Loan` relationship configured

✔ `LoanTransactions` DbSet added

✔ EF Core migration completed successfully

✔ PostgreSQL database updated

✔ `ILoanTransactionRepository` created

✔ `LoanTransactionRepository` created

✔ `LoanTransactionService` created

✔ `LoanTransactionResponse` created

✔ `LoanTransactionsController` created

✔ Create Transaction API implemented

✔ Get All Transactions API implemented

✔ Get Transaction By ID API implemented

✔ Get Transactions By Loan API implemented

✔ Delete Transaction API implemented

✔ Repayment validation implemented

✔ Loan balance automatically updated when transactions are created

✔ Loan balance automatically restored when transactions are deleted

✔ Transaction type names added to API responses

✔ Payment method names added to API responses

✔ Transaction API tested successfully

## Transaction Types

The system currently supports:

1. Disbursement
2. Repayment
3. Interest
4. Penalty
5. Adjustment
6. Refund

## Payment Methods

The system currently supports:

1. Cash
2. Bank Transfer
3. POS
4. Mobile Money
5. Cheque
6. Wallet
7. Other

## API Endpoints

### Create Transaction

`POST /api/loan-transactions`

Creates a new loan transaction and automatically updates the loan balance.

### Get All Transactions

`GET /api/loan-transactions`

Returns all transactions.

### Get Transaction

`GET /api/loan-transactions/{id}`

Returns a specific transaction.

### Get Loan Transaction History

`GET /api/loan-transactions/loan/{loanId}`

Returns all transactions belonging to a particular loan.

### Delete Transaction

`DELETE /api/loan-transactions/{id}`

Deletes a transaction and reverses its effect on the loan balance.

## Validation Implemented

The system prevents:

* Zero or negative transaction amounts
* Transactions against non-existent loans
* Transactions against closed loans
* Repayments greater than the current loan balance
* Invalid transaction types

## Testing Result

A repayment transaction of ₦100,000 was successfully created against the Business Capital loan.

Before repayment:

* Principal: ₦500,000
* Balance: ₦500,000

After repayment:

* Repayment: ₦100,000
* Balance: ₦400,000

After deleting the repayment transaction:

* Balance returned to: ₦500,000

This confirmed that transaction creation and transaction reversal are both functioning correctly.

## Status

**Completed successfully.**

The Loan Transactions foundation is now operational and ready to support future repayment history, financial reporting, dashboards, and offline synchronization.


# Sprint 19 — Loan Transactions

## Objective

Implement the transaction system required to record and manage financial activity against loans.

## Completed

### Domain

* Created `LoanTransaction` entity.
* Added `LoanTransactionType` enum.
* Added `PaymentMethod` enum.
* Established relationship between `Loan` and `LoanTransaction`.
* Added `LoanTransactions` DbSet.
* Added EF Core configuration.
* Added database migration.

### Transaction Types

The system supports:

1. Disbursement
2. Repayment
3. Interest
4. Penalty
5. Adjustment
6. Refund

### Payment Methods

The system supports:

1. Cash
2. Bank Transfer
3. POS
4. Mobile Money
5. Cheque
6. Wallet
7. Other

### Infrastructure

Implemented:

* `ILoanTransactionRepository`
* `LoanTransactionRepository`

Repository operations include:

* Create transaction
* Get transaction by ID
* Get all transactions
* Get transactions by loan
* Delete transaction
* Save changes

### Application Layer

Implemented:

* `CreateLoanTransactionRequest`
* `LoanTransactionResponse`
* `LoanTransactionService`

The service provides:

* Transaction creation
* Disbursement processing
* Repayment processing
* Interest processing
* Penalty processing
* Adjustment processing
* Refund processing
* Transaction history retrieval
* Transaction deletion
* Automatic loan balance calculation

### Business Rules

The transaction service validates:

* Transaction amount must be greater than zero.
* Loan must exist.
* Closed loans cannot receive new transactions.
* Repayment cannot exceed the current loan balance.
* Loan balance is automatically updated after a transaction.
* Deleting a transaction reverses its effect on the loan balance.

### API

Implemented:

`POST /api/loan-transactions`

Create a transaction.

`GET /api/loan-transactions`

Get all transactions.

`GET /api/loan-transactions/{id}`

Get one transaction.

`GET /api/loan-transactions/loan/{loanId}`

Get transaction history for a specific loan.

`DELETE /api/loan-transactions/{id}`

Delete a transaction and reverse its financial effect.

### API Response Improvements

Transaction responses were changed from returning complete EF Core entities to dedicated response DTOs.

This prevents:

* Circular JSON references
* Excessively large API responses
* Unnecessary database relationship data being exposed

The API now returns useful human-readable values such as:

* `transactionTypeName`
* `paymentMethodName`

### Testing

Successfully tested:

* Transaction creation
* Repayment transaction
* Loan balance reduction
* Repayment-over-balance validation
* Get all transactions
* Get transaction by ID
* Get transactions by loan
* Delete transaction
* Loan balance restoration after transaction deletion

### Example Test

Original loan balance:

`₦500,000`

Repayment:

`₦100,000`

Resulting balance:

`₦400,000`

After deleting the repayment transaction:

`₦500,000`

### Build Verification

The complete backend solution builds successfully:

* LoanLedger.Domain
* LoanLedger.Tests
* LoanLedger.Application
* LoanLedger.Infrastructure
* LoanLedger.API

## Status

## Sprint 19 – Loan Transaction Accounting Enhancement: Adjustment & Waiver Support

### Date

August 2026

### Objective

Extend the LoanLedger transaction system to support more realistic loan accounting, particularly lender adjustments and forgiveness/waiver of outstanding loan amounts.

### Work Completed

The loan transaction system was enhanced with two additional concepts:

#### 1. Adjustment Direction

Adjustments can now explicitly indicate whether they:

* **Increase** the outstanding loan balance
* **Decrease** the outstanding loan balance

Implemented through:

`LoanLedger.Domain.Enums.AdjustmentDirection`

```text
Increase = 1
Decrease = 2
```

The `AdjustmentDirection` property is nullable and is only required when the transaction type is `Adjustment`.

#### 2. Waiver Type

The transaction system was extended to support lender forgiveness of outstanding amounts.

Implemented through:

`LoanLedger.Domain.Enums.WaiverType`

A waiver represents an amount that the lender has decided to forgive. The forgiven amount reduces the outstanding loan balance without being treated as a repayment.

The system supports identifying what was waived, such as principal, interest, penalty, or other supported waiver categories.

### Loan Transaction Entity

`LoanTransaction` now contains:

* `AdjustmentDirection`
* `WaiverType`

Both properties are nullable because they only apply to their respective transaction types.

### Transaction Accounting Rules

The centralized balance-effect logic now handles:

| Transaction Type      | Balance Effect |
| --------------------- | -------------: |
| Disbursement          |       Increase |
| Repayment             |       Decrease |
| Interest              |       Increase |
| Penalty               |       Increase |
| Adjustment – Increase |       Increase |
| Adjustment – Decrease |       Decrease |
| Refund                |       Decrease |
| Waiver                |       Decrease |

The balance is prevented from becoming negative.

When the resulting balance reaches zero, the loan is automatically marked as closed.

### Update and Delete Support

Transaction update and deletion logic was updated to use the same balance-effect rules.

This ensures that:

* Updating an adjustment correctly reverses the previous adjustment.
* Updating a waiver correctly reverses the previous waiver before applying the new transaction.
* Deleting a transaction reverses its original financial effect.
* Loan status is recalculated after changes.
* Closed loans can be reopened when a transaction change causes the balance to become greater than zero.

### Database Migration

A new Entity Framework Core migration was created:

`AddAdjustmentAndWaiverSupport`

The migration added the following nullable columns to `LoanTransactions`:

* `AdjustmentDirection`
* `WaiverType`

The migration was successfully applied to the PostgreSQL database.

### API Verification

The implementation was tested through the API.

Successful tests included:

#### Principal Waiver

```text
Transaction Type: Waiver
Waiver Type: Principal
Amount: ₦20,000
Effect: Balance decreased by ₦20,000
```

#### Adjustment Increase

```text
Transaction Type: Adjustment
Direction: Increase
Amount: ₦10,000
Effect: Balance increased by ₦10,000
```

#### Adjustment Decrease

```text
Transaction Type: Adjustment
Direction: Decrease
Amount: ₦5,000
Effect: Balance decreased by ₦5,000
```

The API correctly returned the transaction type names, adjustment direction, waiver type, amounts, references, dates, and descriptions.

### Build Verification

The complete backend solution was successfully built:

```text
LoanLedger.Domain       succeeded
LoanLedger.Tests        succeeded
LoanLedger.Application  succeeded
LoanLedger.Infrastructure succeeded
LoanLedger.API          succeeded
```

Result:

```text
Build succeeded.
```

### Outcome

The LoanLedger backend now supports a more complete financial transaction model in which a lender can:

* record repayments;
* add interest;
* add penalties;
* make balance adjustments;
* increase or decrease a balance through adjustments;
* issue refunds;
* forgive part or all of an outstanding amount through waivers;
* identify the nature of a waiver;
* maintain an accurate current loan balance.

### Next Development Stage

The next major stage is:

**Reporting & PDF Export**

This will support individual-loan reports, periodic reports, custom date-range reports, and complete user/account export.

# Sprint 20 – Loan Summary & Financial Intelligence

## Objectives

- Create loan summary response
- Calculate current loan balance
- Calculate total repayments
- Calculate total interest
- Calculate total penalties
- Count loan transactions
- Track last payment date
- Display loan contact and category
- Display loan status
- Create loan summary API endpoint
- Test summary against real transaction data

## Completed

✔ LoanSummaryResponse created
✔ Loan summary service implemented
✔ Current balance calculation verified
✔ Total repayment calculation verified
✔ Total interest calculation verified
✔ Total penalty calculation verified
✔ Transaction count verified
✔ Last payment date verified
✔ Contact name included
✔ Loan category included
✔ Loan status included
✔ Loan summary API endpoint tested
✔ Repayment transaction tested
✔ Interest transaction tested
✔ Penalty transaction tested
✔ Adjustment transaction tested
✔ Refund transaction tested
✔ Transaction deletion and balance reversal tested
✔ Duplicate transaction reference validation tested

## Verification Result

Loan: Business Capital

Principal Amount: ₦500,000
Total Repaid: ₦250,000
Total Interest: ₦25,000
Total Penalty: ₦10,000
Current Balance: ₦305,000
Transaction Count: 7
Status: Active

All tested transaction operations produced the expected results.

## Status

Completed successfully.

Loan Summary and Financial Intelligence are now operational and verified through API testing.

# Sprint 21 – Workspace Architecture

## Status

Planned / Next Development Stage

## Objective

Introduce the Workspace layer into LoanLedger so that one user can manage different financial environments independently.

## Final Workspace Types

LoanLedger Version 1 supports four workspace types:

1. Individual
2. Business
3. Cooperative
4. Organization

## Workspace Principle

A workspace represents the financial environment in which records are managed.

Workspace type must remain separate from loan type or agreement type.

For example:

An Individual workspace may contain:
- Personal loans
- Informal loans
- Salary advances
- Other financial agreements

A Business workspace may contain:
- Business credit
- Customer credit
- Business loans
- Other financial agreements

A Cooperative workspace may contain:
- Member loans
- Cooperative financial arrangements
- Member repayments

An Organization workspace may contain:
- Staff financial arrangements
- Organization loans
- Other approved financial relationships

## Architectural Direction

The application will use the following ownership structure:

User
↓
Workspace
↓
Contacts
↓
Loan Categories
↓
Loans
↓
Loan Transactions

## Multiple Workspaces

A single user may own multiple workspaces.

Example:

User
├── Individual Workspace
├── Business Workspace
├── Cooperative Workspace
└── Organization Workspace

Each workspace maintains its own financial records.

## Data Isolation

Records belonging to one workspace must not automatically appear in another workspace.

For example:

A loan created inside a Business workspace must not appear in the user's Individual workspace unless the system explicitly supports a future transfer or sharing operation.

## Current Backend Position

The existing backend already supports:

- Authentication
- Users
- Contacts
- Loan Categories
- Loans
- Loan Transactions
- Transaction accounting
- Loan summaries

The Workspace layer has not yet been implemented in the backend.

## Next Implementation

The next backend stage will implement:

- Workspace entity
- Workspace type enum
- Workspace repository
- Workspace service
- Workspace API
- Workspace membership/ownership rules
- Workspace-aware data relationships
- Workspace authorization

## Status

The existing financial engine remains valid.

Workspace implementation will be added as an ownership and isolation layer above the existing financial modules.


# Development Journal — Current Status Update
**Update date:** 18 August 2026

> Append this section after the historical Sprint 21 material. Do not delete the historical record.

# Sprint 22 – Witness Association and Multiple Witness Testing

## Status

Completed

## Objective

Complete the Witness ↔ Loan relationship API and verify the maximum three-witness rule.

## Completed

- LoanWitness entity confirmed.
- LoanWitness repository confirmed.
- LoanWitness service confirmed.
- LoanWitness controller confirmed.
- User ownership validation confirmed.
- Duplicate witness protection confirmed.
- Maximum of 3 witnesses per loan confirmed.
- Automatic witness order assignment confirmed.
- Explicit witness order validation confirmed.
- Witness order uniqueness confirmed.
- Witness retrieval by loan confirmed.
- Witness removal confirmed.

## Test Result

A test loan was successfully associated with:

1. Aisha Bello — order 1
2. Muhammad Usman — order 2
3. Fatima Abdullahi — order 3

Attempting to attach a duplicate witness correctly returned:

`This witness is already attached to the loan.`

Attempting to reuse an occupied witness order correctly returned:

`Witness order 1 is already in use.`

Removal also returned:

`Witness removed from loan successfully.`

## Conclusion

The Witness ↔ Loan lifecycle is operational.

# Sprint 23 – Attachment Foundation

## Status

Current / In Progress

## Immediate Problem

The current build fails in:

`LoanLedgerDbContext.cs(399,22)`

with:

`'Loan' does not contain a definition for 'Attachments'`

Current repository inspection shows that Attachment entity/application/infrastructure/API files do not yet exist.

## Next Objective

Inspect the existing DbContext Attachment configuration, determine whether it is a partial/accidental configuration, then implement the Attachment foundation consistently across Domain, Application, Infrastructure and API.

## Target Attachment Lifecycle

- Attachment metadata
- Loan relationship
- Repository
- Service
- API
- Database migration
- Storage abstraction
- PDF/image/audio/voice metadata tests
- Storage tests

## Rule

Do not proceed to Workspace until Attachment foundation and its initial lifecycle tests are stable.

## 7 September 2026 — Flutter Android Foundation Successfully Completed

### Android Development Environment
The Flutter Android development foundation was successfully configured and verified.

Confirmed environment:
- Flutter 3.47.2
- Dart 3.13.2
- Android SDK 35.0.0
- Android Build Tools 36
- Android NDK 28.2.13676358
- OpenJDK 17.0.20.1
- Gradle 9.3.1
- Android Gradle Plugin 9.1.0
- Physical Android device: Infinix PR652B
- Android version: Android 11 / API 30

### Gradle and Network Verification
Earlier Gradle dependency-download problems were resolved after moving to a stable Wi-Fi connection.

Verified:
- Maven Central connectivity
- Google Maven connectivity
- Gradle 9.3.1 execution
- `gradlew help --stacktrace` completed successfully
- Required Android NDK was installed successfully

### Flutter Project Verification
The Flutter project was cleaned and dependencies were successfully restored.

Commands verified:
- `flutter clean`
- `flutter pub get`
- `flutter devices`

The physical Infinix PR652B device was detected and authorized through ADB.

### First Successful Android Build
The Flutter application was successfully built and installed on the physical Android device.

Verified:
- `flutter run`
- Debug APK generated successfully
- APK installed successfully
- Flutter application launched on the physical device
- Flutter Demo Home Page displayed correctly
- Dart VM Service connected successfully

This confirms that the Android/Flutter development foundation is operational.

### Backend Verification Before Flutter Transition
The backend was re-verified before beginning the Flutter application phase.

Results:
- `dotnet build` — successful
- Domain project — successful
- Application project — successful
- Infrastructure project — successful
- API project — successful
- Tests project — successful
- `dotnet test` — 1 test passed, 0 failed

### Milestone Status
The project has now transitioned from backend foundation and Android environment setup into the actual Flutter application development phase.

Next major phase:
- Replace the Flutter demo application with the LoanLedger application shell.
- Establish LoanLedger branding, navigation, theme, authentication foundation, and API integration architecture.
