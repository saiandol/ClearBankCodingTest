using System.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class DataStoreConfig : IDataStoreConfig
    {
        public bool UseBackupDataStore => ConfigurationManager.AppSettings["DataStoreType"].Trim().ToLowerInvariant() == "Backup";
    }
}
