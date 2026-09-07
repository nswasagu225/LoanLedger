# LoanLedger Database Design

## Main Entities

1. Users
2. Workspaces
3. Contact Wallet
4. Contact Addresses
5. Contact Phones
6. Contact Emails
7. Loan Agreements
8. Repayments
9. Witnesses
10. Attachments
11. Reminders
12. Categories
13. Tags
14. Notifications
15. Activity Logs
16. Currencies
17. Report Templates

## Design Principles

- Flexible
- Offline First
- Cloud Synchronization
- Multi-Currency
- Multi-Workspace
- Smart Agreements
- Contact Wallet
- Guided User Experience
- Progressive Disclosure
- Extensible Architecture


# Database Design — Current Implementation Update
**As of:** 18 August 2026

## Implemented / Confirmed

Current backend database architecture includes the financial foundation and the LoanWitness relationship.

### LoanWitness

Relationship:

```text
Loans
  |
  | 1-to-many
  v
LoanWitness
  ^
  | many-to-1
  |
Witnesses
```

The relationship supports:

- LoanId
- WitnessId
- WitnessOrder
- CreatedAt

The service limits a loan to three witnesses.

## Attachment — Not Yet Implemented

The database design document lists Attachments as a planned main entity, but the current code inspection found no Attachment entity or Attachment application/infrastructure/API files.

However, the current `LoanLedgerDbContext` contains a reference to:

`loan.Attachments`

This is currently causing the build failure:

`CS1061: 'Loan' does not contain a definition for 'Attachments'`

Therefore the next database task is to inspect and reconcile the existing DbContext configuration before creating a migration.

Do not create an Attachment migration until the entity and relationship design have been confirmed.
