using Npgsql;

namespace LibraryManagementSystemHtmx.Data;

public static class MigrationRunner
{
  public static void RunMigrations(string connectionString)
  {
    string migrationsPath = Path.Combine(Directory.GetCurrentDirectory(), "Migrations");
    string[] migrationFiles = Directory.GetFiles(migrationsPath, "*.sql");

    using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
    connection.Open();

    foreach (string migrationFile in migrationFiles)
    {
        string query = File.ReadAllText(migrationFile);
        using NpgsqlCommand command = new NpgsqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    Console.WriteLine("Migrations executed successfully.");
  }
}