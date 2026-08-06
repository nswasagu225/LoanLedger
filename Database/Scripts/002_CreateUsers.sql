-- ===========================================
-- LoanLedger
-- Table: Users
-- Version 0.1.0
-- ===========================================

CREATE TABLE Users
(
    UserId UUID PRIMARY KEY,

    FullName VARCHAR(150) NOT NULL,

    Email VARCHAR(150) NOT NULL UNIQUE,

    PhoneNumber VARCHAR(30),

    PasswordHash TEXT NOT NULL,

    ProfilePhoto TEXT,

    Country VARCHAR(100),

    TimeZone VARCHAR(100),

    PreferredCurrency VARCHAR(10),

    Language VARCHAR(20),

    IsVerified BOOLEAN NOT NULL DEFAULT FALSE,

    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt TIMESTAMP
);