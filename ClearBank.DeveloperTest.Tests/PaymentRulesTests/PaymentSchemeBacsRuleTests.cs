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
    public class PaymentSchemeBacsRuleTests
    {
        private IPaymentRule _paymentSchemeBacsRule;

        [TestInitialize]
        public void Setup()
        {
            _paymentSchemeBacsRule = new PaymentSchemeBacsRule();
        }

        [TestMethod]
        public void WhenBacsAccountStatusWithAnyStatus_WithBalance100_AndBacsRequestForAmmount99_Is_Made_Then_RulePasses()
        {
            var account = new Account()
            {
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = 100,
                Status = AccountStatus.Live
            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditorAccountNumber",
                DebtorAccountNumber = "debtorAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };

            var ruleResult = _paymentSchemeBacsRule.Apply(account, makePaymentRequest);


            ruleResult.Should().BeTrue();


        }
        
        [TestMethod]
        public void WhenBacsAccountStatusWithAnyStatus_WithBalance100_ButChapsRequestForAmmount99_Is_Made_Then_RuleFails()
        {
            var account = new Account()
            {
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = 100,
                Status = AccountStatus.Live
            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditorAccountNumber",
                DebtorAccountNumber = "debtorAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var ruleResult = _paymentSchemeBacsRule.Apply(account, makePaymentRequest);


            ruleResult.Should().BeFalse();


        }
        
        [TestMethod]
        public void WhenBacsAccountStatusWithAnyStatus_WithBalance100_ButBacsRequestForAmmount200_Is_Made_Then_RuleStillPasses()
        {
            var account = new Account()
            {
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = 100,
                Status = AccountStatus.Disabled
            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 200,
                CreditorAccountNumber = "creditorAccountNumber",
                DebtorAccountNumber = "debtorAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };

            var ruleResult = _paymentSchemeBacsRule.Apply(account, makePaymentRequest);


            ruleResult.Should().BeTrue();


        }
        
        [TestMethod]
        public void WhenChapsAccountStatusWithAnyStatus_WithBalance100_AndBacsRequestForAmmount99_Is_Made_Then_RuleFails()
        {
            var account = new Account()
            {
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 100,
                Status = AccountStatus.Live
            };

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditorAccountNumber",
                DebtorAccountNumber = "debtorAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };

            var ruleResult = _paymentSchemeBacsRule.Apply(account, makePaymentRequest);


            ruleResult.Should().BeFalse();


        }
        
        [TestMethod]
        public void WhenAccountIsNull_AndBacsRequestForAmmount99_Is_Made_Then_RulePasses()
        {
            Account account = null;
            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 9,
                CreditorAccountNumber = "creditorAccountNumber",
                DebtorAccountNumber = "debtorAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var ruleResult = _paymentSchemeBacsRule.Apply(account, makePaymentRequest);


            ruleResult.Should().BeFalse();


        }
        
        
    }
}
