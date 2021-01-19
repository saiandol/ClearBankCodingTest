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
    public class PaymentSchemeChapsRuleTests
    {
        private IPaymentRule _paymentSchemeChapsRule;

        [TestInitialize]
        public void Setup()
        {
            _paymentSchemeChapsRule = new PaymentSchemeChapsRule();
        }

        [TestMethod]
        public void WhenChapsAccountStatusIsLive_WithBalance100_AndChapsRequestForAmmount99_Is_Made_Then_RulePasses()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var ruleResult = _paymentSchemeChapsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeTrue();
        }

        [TestMethod]
        public void WhenChapsAccountStatusIs_NotLive_WithBalance100_AndChapsRequestForAmmount99_Is_Made_Then_RuleFails()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.InboundPaymentsOnly

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var ruleResult = _paymentSchemeChapsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeFalse();
        }
        
        [TestMethod]
        public void WhenChapsAccountStatusIsLive_WithBalance100_AndBacsRequestForAmmount99_Is_Made_Then_RuleFails()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Status = AccountStatus.Live

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var ruleResult = _paymentSchemeChapsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeFalse();
        }
        
        [TestMethod]
        public void WhenAccountIsNull_AndChapsRequestForAmmount99_Is_Made_Then_RuleFails()
        {
            Account account = null;
            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var ruleResult = _paymentSchemeChapsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeFalse();
        }
        
        [TestMethod]
        public void WhenChapsAccountStatusIsLive_WithBalance100_AndChapsRequestForAmmount200_Is_Made_Then_RuleStillPasses()
        {
            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live

            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 200,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var ruleResult = _paymentSchemeChapsRule.Apply(account, makePaymentRequest);

            ruleResult.Should().BeTrue();
        }
    }
}
