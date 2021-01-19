using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Extensions
{
    public static class AccountExtensions
    {
        public static void Withdraw(this Account account, decimal requestAmount)
        {
            account.Balance -= requestAmount;
        }
    }
}
