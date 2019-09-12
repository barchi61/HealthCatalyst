using PeopleSearch.Infrastructure.Services;
using System.Data.Entity;
using System.Data.SQLite;

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
