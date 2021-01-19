using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services.PaymentRules;
using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ClearBank.DeveloperTest.Services.Extensions;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;
        private readonly List<IPaymentRule> _paymentRules;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory, List<IPaymentRule> paymentRules)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
            _paymentRules = paymentRules;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {

            var accountDataStore = _accountDataStoreFactory.Create();

            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult
            {
                Success = _paymentRules.Any(rule => rule.Apply(account, request))
            };

            if (result.Success)
            {
                account.Withdraw(request.Amount);

                accountDataStore.UpdateAccount(account);
            }

            return result;
        }

        
    }
}
