# LoanLedger Testing Checkpoint — 26 August 2026

## Confirmed Recent Results

### Loan Date Validation

Input with a future loan date was rejected:

```text
Loan date cannot be in the future.
```

### Loan Item Requirement

A loan without an item was rejected:

```text
At least one loan item is required.
```

### Valid Loan Creation

A valid loan was created successfully.

### Partial Repayment

A ₦100,000 loan received a ₦20,000 repayment.

Result:
- Balance: ₦80,000
- Total repaid: ₦20,000
- Status: Active

### Manual Close With Outstanding Balance

Rejected correctly:

```text
A loan cannot be closed while it has an outstanding balance.
```

### Full Repayment

A further ₦80,000 repayment reduced the balance to zero.

Result:
- Balance: ₦0
- Loan automatically closed
- `isClosed = true`
- `closedAt` populated

### Repeated Close

Rejected correctly:

```text
This loan is already closed.
```

### Direct Reopen of Settled Loan

Rejected correctly:

```text
A fully repaid loan cannot be reopened.
```

## Recommended Final Policy

These tests should be treated as completed validation evidence.

Future work should focus on:
- transaction correction workflows;
- audit/history;
- authorization;
- workspace isolation;
- edge cases introduced by new code.

Do not repeat the same manual close/reopen tests unless the implementation changes.
