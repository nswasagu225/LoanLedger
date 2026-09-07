using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Contacts;

public static class ContactTrustCalculator
{
    public static ContactTrustStatus GetStatus(decimal score)
    {
        if (score >= 70)
            return ContactTrustStatus.Trusted;

        if (score >= 40)
            return ContactTrustStatus.Caution;

        return ContactTrustStatus.HighRisk;
    }
}