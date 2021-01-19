using ClearBank.DeveloperTest.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClearBank.DeveloperTest.Tests
{
    [TestClass]
    public class AccountDataStoreFactoryTests
    {
        private IAccountDataStoreFactory _accountDataStoreFactory;
        private Mock<IDataStoreConfig> _dataStoreConfigMock;

        [TestInitialize]
        public void Setup()
        {
            _dataStoreConfigMock = new Mock<IDataStoreConfig>();
            _accountDataStoreFactory = new AccountDataStoreFactory(_dataStoreConfigMock.Object);
        }

        [TestMethod]
        public void WhenUseBackupDataStore_IsTrue_Factory_Returns_BackupAccountDataStore()
        {
            //arrange
            _dataStoreConfigMock.Setup(c => c.UseBackupDataStore).Returns(true);

            //act
            var accountDataStore = _accountDataStoreFactory.Create();
            
            //assert
            accountDataStore.Should().NotBeNull();
            accountDataStore.Should().BeOfType(typeof(BackupAccountDataStore));
        }
        
        [TestMethod]
        public void WhenUseBackupDataStore_IsFalse_Factory_Returns_AccountDataStore()
        {
            //arrange
            _dataStoreConfigMock.Setup(c => c.UseBackupDataStore).Returns(false);

            //act
            var accountDataStore = _accountDataStoreFactory.Create();

            //assert
            accountDataStore.Should().NotBeNull();
            accountDataStore.Should().BeOfType(typeof(AccountDataStore));
        }

    }
}
