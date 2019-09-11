using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using PeopleSearch.Infrastructure.Services;

namespace PeopleSearch.Data.EDM
{
    public class PeopleContext : DbContext
    {
        public PeopleContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource =  ApplicationSettings.DataPath, ForeignKeys = true }.ConnectionString
            }, true)
        {
        }

        public DbSet<People> People { get; set; }

    }
}
