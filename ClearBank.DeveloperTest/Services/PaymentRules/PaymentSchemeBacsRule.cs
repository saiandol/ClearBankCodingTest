using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentRules
{
    public class PaymentSchemeBacsRule : IPaymentRule
    {
        public bool Apply(Account account, MakePaymentRequest makePaymentRequest)
        {
            return account != null && (makePaymentRequest.PaymentScheme == PaymentScheme.Bacs && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs));
        }
    }
}
