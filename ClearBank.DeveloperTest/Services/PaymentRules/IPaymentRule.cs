using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services.PaymentRules
{
    public interface IPaymentRule
    {
        bool Apply(Account account, MakePaymentRequest makePaymentRequest);
    }
}
