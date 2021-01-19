using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.PaymentRules;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClearBank.DeveloperTest.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        private IPaymentService _paymentService;
        private Mock<IAccountDataStoreFactory> _accountDataStoreFactoryMock;
        private List<IPaymentRule> _paymentRules;
        private Mock<IAccountDataStore> _accountDataStoreMock;

        [TestInitialize]
        public void Setup()
        { 
            _accountDataStoreFactoryMock = new Mock<IAccountDataStoreFactory>();
            _accountDataStoreMock = new Mock<IAccountDataStore>();
           
            _accountDataStoreFactoryMock.Setup(x => x.Create()).Returns(_accountDataStoreMock.Object);
            _paymentRules = new List<IPaymentRule>()
            {
                new PaymentSchemeBacsRule(),
                new PaymentSchemeChapsRule(),
                new PaymentSchemeFasterPaymentsRule()
            };
           _paymentService = new PaymentService(_accountDataStoreFactoryMock.Object, _paymentRules);
        }

        #region ChapsPaymentTests

        [TestMethod]
        public void WhenChapsRequestForAmmount99_Is_Made_On_ChapsAccountStatusWithLiveStatus_WithBalance100_Then_MakePaymentResult_IsASuccess()
        {
            //arrange
            var request = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            //act
            var makePaymentResult = _paymentService.MakePayment(request);

            //assert
            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(1);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void WhenChapsRequestForAmmount99_Is_Made_On_ChapsAccountStatus_Disabled_WithBalance100_Then_MakePaymentResult_Fails()
        {
            //assert
            var request = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Disabled

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            //act
            var makePaymentResult = _paymentService.MakePayment(request);

            //assert
            makePaymentResult.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [TestMethod]
        public void WhenChapsRequestForAmmount99_Is_Made_On_ChapsAccountStatus_InboundPaymentsOnly_AndWithBalance100_Then_MakePaymentResult_Fails()
        {
            //assert
            var request = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.InboundPaymentsOnly

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            //act
            var makePaymentResult = _paymentService.MakePayment(request);

            //assert
            makePaymentResult.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [TestMethod]
        public void WhenChapsRequestForAmmount200_Is_Made_On_ChapsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_Succeeds()
        {
            //assert
            var request = new MakePaymentRequest()
            {
                Amount = 200,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            //act
            var makePaymentResult = _paymentService.MakePayment(request);

            //assert
            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(-100);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void WhenChapsRequestForAmmount200_Is_Made_On_ChapsAccountThatDoesNotExit_Then_MakePaymentResult_Fails()
        {
            //assert
            var request = new MakePaymentRequest()
            {
                Amount = 200,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            Account account = null;

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            //act
            var makePaymentResult = _paymentService.MakePayment(request);

            //assert
            makePaymentResult.Success.Should().BeFalse();
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        #endregion


        #region FasterPaymentTests

        [TestMethod]
        public void WhenFasterPaymentsRequestForAmmount99_Is_Made_On_FasterPaymentsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_Succeeds()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(1);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void WhenFasterPaymentsRequestForAmmount99_Is_Made_On_FasterPaymentsAccountStatus_Disabled_AndWithBalance100_Then_MakePaymentResult_Succeeds()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Disabled

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(1);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void WhenFasterPaymentsRequestForAmmount99_Is_Made_On_FasterPaymentsAccountStatus_InboundPaymentsOnly_AndWithBalance100_Then_MakePaymentResult_Succeeds()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 99,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.InboundPaymentsOnly

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(1);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void WhenFasterPaymentsRequestForAmmount100_Is_Made_On_FasterPaymentsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_Succeeds()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(0);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void WhenFasterPaymentsRequestForAmmount200_Is_Made_On_FasterPaymentsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_Fails()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 200,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
        
        [TestMethod]
        public void WhenNonFasterPaymentsRequestForAmmount200_Is_Made_On_FasterPaymentsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_Fails()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Chaps
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
        
        [TestMethod]
        public void WhenFasterPaymentsRequestForAmmount200_Is_Made_On_An_Account_ThatDoesNotExit_Then_MakePaymentResult_Fails()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            Account account = null;

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeFalse();
           
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }



        #endregion

        #region BacsPaymentTests

        [TestMethod]
        public void WhenBacsRequestForAmmount100_Is_Made_On_BacsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_Succeeds()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(0);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void WhenChapsRequestForAmmount100_Is_Made_On_BacsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_Fails()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [TestMethod]
        public void WhenBacsRequestForAmmount100_Is_Made_On_BacsAccount_ThatDoesNotExist_Then_MakePaymentResult_Fails()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };

            Account account = null;

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeFalse();

            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [TestMethod]
        public void WhenBacsRequestForAmmount200_Is_Made_On_BacsAccountStatus_Live_AndWithBalance100_Then_MakePaymentResult_stillSucceeds()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 200,
                CreditorAccountNumber = "creditAccountNumber",
                PaymentDate = DateTime.Today,
                PaymentScheme = PaymentScheme.Bacs
            };

            var account = new Account()
            {
                Balance = 100,
                AccountNumber = "accountNumber",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Status = AccountStatus.Live

            };

            _accountDataStoreMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var makePaymentResult = _paymentService.MakePayment(request);

            makePaymentResult.Success.Should().BeTrue();
            account.Balance.Should().Be(-100);
            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        #endregion

    }
}
