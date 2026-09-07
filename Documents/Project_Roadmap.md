# LoanLedger Roadmap

## Phase 1 — Environment Setup

✅ Completed

- Project setup
- Development environment
- Git
- GitHub
- Documentation

---

## Phase 2 — Backend Foundation

✅ Completed

- Clean Architecture
- ASP.NET Core
- C#
- Dependency Injection
- Repository Pattern
- Unit of Work

---

## Phase 3 — Database

✅ Completed

- PostgreSQL
- Entity Framework Core
- Database migrations
- LoanLedgerDB
- Database relationships

---

## Phase 4 — Authentication

✅ Completed

- User registration
- Login
- Email login
- Phone login
- Password hashing
- JWT authentication
- Authorization foundation

---

## Phase 5 — Contact Center

✅ Completed

- Contact entity
- Contact repository
- Contact service
- Contact API
- Duplicate validation

---

## Phase 6 — Financial Reference Data

✅ Completed

- Loan Categories
- Category repository
- Category service
- Category API
- Duplicate category validation

---

## Phase 7 — Financial Agreements

✅ Completed

### Loan Accounts

- Loan entity
- Loan relationships
- Loan repository
- Loan DTOs
- Loan service
- Loan controller
- Loan creation
- Loan retrieval

### Loan Transactions

- Transaction entity
- Transaction types
- Payment methods
- Transaction repository
- Transaction service
- Transaction API
- Transaction history
- Repayment validation
- Interest
- Penalty
- Adjustment
- Refund
- Waiver
- Automatic balance calculation
- Transaction reversal
- Transaction deletion
- Automatic loan closure
- Loan reopening

### Loan Summary

- Current balance
- Total repayments
- Total interest
- Total penalties
- Transaction count
- Last payment date
- Loan status
- Contact information
- Category information

---

# Phase 8 — Workspace Management

⏳ Next

## Workspace Types

- Individual
- Business
- Cooperative
- Organization

## Objectives

- Workspace entity
- Workspace type enum
- Workspace ownership
- Workspace repository
- Workspace service
- Workspace controller
- Workspace creation
- Workspace retrieval
- Workspace update
- Workspace activation/deactivation
- Workspace-aware authorization
- Workspace data isolation

## Architecture

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
Transactions

---

# Phase 9 — Reporting & Financial Export

⏳ Planned

- Individual loan reports
- Workspace reports
- All-loan reports
- Weekly reports
- Monthly reports
- Custom date-range reports
- Opening balance
- Period transactions
- Closing balance
- Repayment summaries
- Interest summaries
- Penalty summaries
- Adjustment summaries
- Waiver summaries
- Refund summaries
- PDF generation
- Image reports
- Complete account export
- Historical record preservation

---

# Phase 10 — Offline Synchronization

⏳ Planned

- SQLite local database
- Offline record creation
- Offline transaction entry
- Synchronization queue
- Conflict detection
- Conflict resolution
- Server synchronization
- Sync status

---

# Phase 11 — Smart Agreements

⏳ Planned

- Agreement wizard
- Flexible agreement types
- Witnesses
- Attachments
- Smart agreement records
- Agreement history
- Agreement confirmation

---

# Phase 12 — Flutter Mobile Application

⏳ Planned

- Flutter foundation
- Authentication UI
- Workspace selection
- Workspace dashboard
- Contact wallet
- Loan management
- Transaction management
- Reports
- Offline mode
- Synchronization

---

# Phase 13 — Dashboard & Financial Intelligence

⏳ Planned

- Workspace dashboard
- Outstanding balances
- Total lent
- Total borrowed
- Repayment statistics
- Interest statistics
- Penalty statistics
- Waiver statistics
- Transaction trends
- Financial summaries

---

# Phase 14 — Notifications

⏳ Planned

- Due-date notifications
- Repayment reminders
- Agreement reminders
- Workspace notifications
- Transaction confirmations

---

# Phase 15 — Version 1.0 Release

⏳ Planned

- Backend production readiness
- Mobile application
- Offline synchronization
- Reporting
- PDF export
- Security review
- Testing
- Performance testing
- Deployment
- Version 1.0 release

# Roadmap — Current Status Update
**As of:** 18 August 2026

## Immediate Milestone — Attachment Foundation

🔧 Current

### Objectives

- Resolve existing Attachment-related DbContext configuration.
- Create Attachment domain entity.
- Create Loan ↔ Attachment relationship.
- Create repository.
- Create service.
- Create API.
- Create EF Core configuration/migration.
- Establish storage abstraction.
- Test metadata and storage handling.

### Test Types

- PDF
- Image
- Audio
- Voice recording

---

# Next Milestone — Workspace Management

⏳ After Attachment

Workspace types:

- Individual
- Business
- Cooperative
- Organization

Objectives:

- Workspace entity
- Workspace type enum
- Ownership
- Repository
- Service
- Controller
- Creation/retrieval/update
- Activation/deactivation
- Authorization
- Entitlement rules
- Workspace-aware data relationships
- Data isolation

---

# Then

## Loan Description / Details

⏳ Planned

## Reporting & Financial Export

⏳ Planned

## Offline Synchronization

⏳ Planned

## Smart Agreements

⏳ Planned

## Flutter Mobile Application

⏳ Planned

## Dashboard & Financial Intelligence

⏳ Planned

## Notifications

⏳ Planned

## Version 1.0

⏳ Planned
