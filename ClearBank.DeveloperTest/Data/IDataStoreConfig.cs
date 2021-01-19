using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Data
{
    public interface IDataStoreConfig
    {
        bool UseBackupDataStore { get; }
    }
}
