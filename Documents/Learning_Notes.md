# LoanLedger Learning Notes

This notebook contains everything learned during the development of LoanLedger.

---

## Git

Status:
Not Started

---

## Flutter

Status:
Not Started

---

## Dart

Status:
Not Started

---

## ASP.NET Core

Status:
Not Started

---

## PostgreSQL

Status:
Not Started

---

## SQLite

Status:
Not Started

---

## Android Studio

Status:
Not Started

## .NET

### What is .NET?

.NET is Microsoft's software development platform.

### What is the .NET SDK?

The Software Development Kit (SDK) contains the tools needed to create, build, run, and publish .NET applications.

### Why are we using it?

LoanLedger's backend will be built with ASP.NET Core, which runs on .NET.

### Useful Commands

dotnet --version

Shows the installed SDK version.

dotnet --info

Shows detailed SDK and runtime information.
## Understanding .NET

.NET is the software platform.

C# is the programming language.

ASP.NET Core is the web framework.

Visual Studio is the IDE.

The .NET SDK contains the tools needed to create, build, and run .NET applications.

Useful Commands:

dotnet --version

Returns the installed SDK version.

dotnet --info

Shows detailed information about the installed SDK and runtimes.

## Loan Transaction Accounting – Adjustment and Waiver

### Adjustment vs Waiver

An **Adjustment** is used when the recorded outstanding balance needs to be corrected or changed.

An adjustment therefore requires a direction:

* **Increase** – adds an amount to the outstanding balance.
* **Decrease** – removes an amount from the outstanding balance.

A **Waiver**, however, represents a lender's decision to forgive an amount that would otherwise remain payable.

For example:

```text
Outstanding balance = ₦200,000

Lender forgives ₦50,000

Waiver = ₦50,000
New balance = ₦150,000
```

The ₦50,000 is not a repayment because the borrower did not pay it. It is a reduction resulting from the lender forgiving the obligation.

### Why Waiver Type Is Important

A waiver should identify what the lender forgave.

For example:

* Principal waiver
* Interest waiver
* Penalty waiver
* Other supported waiver category

This allows LoanLedger reports to distinguish between money actually repaid and money that was forgiven.

### Centralized Balance Effect

LoanLedger uses a centralized balance-effect method instead of maintaining separate balance calculations throughout the service.

The concept is:

```text
New Balance = Current Balance + Transaction Effect
```

Examples:

```text
Repayment       → negative effect
Interest        → positive effect
Penalty         → positive effect
Adjustment ↑    → positive effect
Adjustment ↓    → negative effect
Refund          → negative effect
Waiver          → negative effect
```

This reduces the possibility of different parts of the application calculating the same transaction differently.

### Important Accounting Principle

A repayment and a waiver are not the same thing.

For example:

```text
₦50,000 repayment
```

means the borrower actually paid ₦50,000.

Whereas:

```text
₦50,000 waiver
```

means the lender forgave ₦50,000.

Both reduce the balance, but they must remain separate transaction types because they have different financial meanings and must appear differently in reports.

### Transaction Reversal

When an existing transaction is updated or deleted, its original balance effect must first be reversed.

For example:

```text
Original:
Adjustment Increase ₦10,000

Reverse:
-₦10,000

Then apply the new transaction.
```

This principle is important because the system must not simply overwrite the transaction and leave the old financial effect in the loan balance.

### Loan Closure

After a transaction changes the balance:

```text
If CurrentBalance = 0
    Loan is closed
Else
    Loan remains active
```

If a transaction is later deleted or changed and the balance becomes greater than zero, the loan can be reopened.

### Reporting Implication

Because LoanLedger distinguishes repayments, adjustments, refunds, interest, penalties, and waivers, future reports can provide a much clearer financial picture.

For example, a report can show:

```text
Original Principal       ₦500,000
Total Repaid             ₦270,000
Interest Added            ₦25,000
Penalties Added           ₦10,000
Adjustments               +₦10,000 / -₦5,000
Amount Waived             ₦20,000
Current Balance           ₦270,000
```

This is much more informative than simply showing a single current balance.

# Workspace Architecture

## What Is a Workspace?

A workspace is an independent financial environment inside LoanLedger.

It allows one user to manage different financial activities separately.

For example, the same user may have:

- Individual workspace
- Business workspace
- Cooperative workspace
- Organization workspace

## Workspace Types

LoanLedger currently supports four workspace types.

### 1. Individual

Used for personal financial relationships.

Examples:

- Friend-to-friend loans
- Personal loans
- Salary advances
- Informal lending

The Individual workspace should remain flexible.

Simple informal loans should not require unnecessary complex fields.

### 2. Business

Used for business financial relationships.

Examples:

- Business credit
- Customer credit
- Business loans
- Other business financial arrangements

### 3. Cooperative

Used for cooperative financial management.

Examples:

- Member loans
- Member repayments
- Cooperative financial records
- Member financial relationships

### 4. Organization

Used by organizations to manage financial relationships.

Examples:

- Staff financial arrangements
- Organizational loans
- Member-related financial records
- Other organizational financial relationships

## Workspace Type vs Loan Type

These are two different concepts.

Workspace Type answers:

"Where is this financial record being managed?"

Loan Type or Agreement Type answers:

"What kind of financial arrangement is this?"

For example:

```text
Workspace:
Business

Agreement:
Business Credit

