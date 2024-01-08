namespace Touride.Framework.Abstractions.Data.Migration
{
    public class DatabaseMigrationsConfiguration
    {
        public const string Section = "Touride.Framework:DatabaseMigrationsConfiguration";

        public bool ApplyDatabaseMigrations { get; set; }

    }
}
