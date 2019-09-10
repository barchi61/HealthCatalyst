using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace PeopleSearch.Data.EDM
{
    public class PeopleContext : DbContext
    {
        private static string _dataPath;
        public static string DataPath
        {
            get
            {
                var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var rootPath = assemblyPath.Replace(@"\PeopleSearch\PeopleSearch.Shell\bin\Debug", "");
                _dataPath = Path.Combine(rootPath, @"Data\People.db");
                return _dataPath;
            }
        }

        public PeopleContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource =  DataPath, ForeignKeys = true }.ConnectionString
            }, true)
        {
        }

        public DbSet<People> People { get; set; }

    }
}
