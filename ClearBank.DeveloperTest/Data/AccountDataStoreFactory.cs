using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        private readonly IDataStoreConfig _dataStoreConfig;

        public AccountDataStoreFactory(IDataStoreConfig dataStoreConfig)
        {
           _dataStoreConfig = dataStoreConfig;
        }

        public IAccountDataStore Create()
        {
            return _dataStoreConfig.UseBackupDataStore ? new BackupAccountDataStore() : (IAccountDataStore)new AccountDataStore();
        }
    }
}
