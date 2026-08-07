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