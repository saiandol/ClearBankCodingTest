using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Services.PaymentRules;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClearBank.DeveloperTest.Tests.PaymentRulesTests
{
    [TestClass]
    public class PaymentSchemeFasterPaymentsRuleTests
    {
        private IPaymentRule _paymentSchemeFasterPaymentsRule;

        [TestInitialize]
        public void Setup()
        {
            _paymentSchemeFasterPaymentsRule = new PaymentSchemeFasterPaymentsRule();
        }

        [TestMethod]
        public void WhenAccountStatusIsLive_WithBalance100_AndFasterPaymentsRequestForAmmount99_Is_Made_Then_RulePasses()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };


            var ruleResult = _paymentSchemeFasterPaymentsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeTrue();

        }
        
        [TestMethod]
        public void WhenAccountStatusIsLive_WithBalance99_AndFasterPaymentsRequestForAmmount100_Is_Made_Then_RuleFails()
        {
            var account = new Account()
            {
                Balance = 99,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };


            var ruleResult = _paymentSchemeFasterPaymentsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeFalse();

        }
        
        [TestMethod]
        public void WhenAccountStatusIsDisabled_WithBalance100_AndBacsRequestForAmmount99_Is_Made_Then_RuleFails()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };


            var ruleResult = _paymentSchemeFasterPaymentsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeFalse();

        }
        
        [TestMethod]
        public void WhenAccountStatusIsLive_WithBalance100_AndFasterPaymentsRequestForAmmount100_Is_Made_Then_RulePasses()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };


            var ruleResult = _paymentSchemeFasterPaymentsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeTrue();

        }
        
        [TestMethod]
        public void WhenAccountIsNull_AndFasterPaymentsRequest_Is_Made_Then_RuleFails()
        {
            Account account = null;

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };


            var ruleResult = _paymentSchemeFasterPaymentsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeFalse();

        }
        
        [TestMethod]
        public void WhenAccountStatusIsDisabled_WithBalance100_AndFasterPaymentsRequestForAmmount99_Is_Made_Then_RulePasses()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Disabled

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };


            var ruleResult = _paymentSchemeFasterPaymentsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeTrue();

        }
    }
}
