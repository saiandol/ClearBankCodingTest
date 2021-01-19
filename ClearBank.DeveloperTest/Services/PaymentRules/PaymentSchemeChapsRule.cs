using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentRules
{
    public class PaymentSchemeChapsRule : IPaymentRule
    {
        public bool Apply(Account account, MakePaymentRequest makePaymentRequest)
        {
            return account != null &&
                   (makePaymentRequest.PaymentScheme == PaymentScheme.Chaps && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)) &&
                   account.Status == AccountStatus.Live;
        }
    }
}
