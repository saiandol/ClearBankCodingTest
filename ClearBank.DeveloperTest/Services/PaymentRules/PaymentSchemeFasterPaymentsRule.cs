using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentRules
{
    public class PaymentSchemeFasterPaymentsRule : IPaymentRule
    {
        public bool Apply(Account account, MakePaymentRequest makePaymentRequest)
        {
            return account != null &&
                makePaymentRequest.PaymentScheme == PaymentScheme.FasterPayments && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
                account.Balance >= makePaymentRequest.Amount;
        }
    }
}
