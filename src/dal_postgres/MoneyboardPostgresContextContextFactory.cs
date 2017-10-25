using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace dal_postgres
{
    public class MoneyboardPostgresContextContextFactory : IDesignTimeDbContextFactory<MoneyboardPostgresContext>
    {
        public const string CONNECTION_STRING = "User ID=webapi;Password=mypostgrespassword!;Host={0};Port=5432;Database=moneyboard;Pooling=true;";

        public MoneyboardPostgresContext CreateDbContext(string[] args)
        {
            var servername = Environment.GetEnvironmentVariable("APP_DB_SERVER");

            if(string.IsNullOrEmpty(servername))
                servername = "localhost";

            var builder = new DbContextOptionsBuilder<dal.MoneyboardContext>();
            builder.UseNpgsql(string.Format(CONNECTION_STRING, servername));
            
            var context = new MoneyboardPostgresContext(builder.Options);
            return context;
        }
    }
}