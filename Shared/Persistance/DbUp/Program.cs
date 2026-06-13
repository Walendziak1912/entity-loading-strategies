using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Persistance.Common.Extensions;
using Shared.Persistance.Common.Providers;
using System.Reflection;

namespace PersistanceDbUp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            
            var outputDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.SetCurrentDirectory(outputDir);
            }
            
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "appsettings.json");
            var mainConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Program>(optional: true)
                .Build();

            var dbSection = configuration.GetSection("Database");
            Console.WriteLine($"Database section exists: {dbSection.Exists()}");
            if (dbSection.Exists())
            {
                Console.WriteLine($"ConnectionString from config: {dbSection["ConnectionString"]}");
            }

            // DI
            var services = new ServiceCollection();
            services.AddDatabaseConfiguration(configuration);
            var provider = services.BuildServiceProvider();
            var dbOptionsProvider = provider.GetRequiredService<IDatabaseOptionsProvider>();
            var dbOptions = dbOptionsProvider.Value;

            if (string.IsNullOrEmpty(dbOptions.ConnectionString))
            {
                Console.WriteLine("Błąd: Brak connection string w konfiguracji!");
                Console.WriteLine("Sprawdź appsettings.json lub dodaj User Secrets.");
                return;
            }

            // Sprawdź i utwórz bazę danych jeśli nie istnieje
            await EnsureDatabaseExistsAsync(dbOptions.ConnectionString, dbOptions.DatabaseName);

            try
            {
                // Migracja bazy danych
                var upgrader = DeployChanges.To
                    .SqlDatabase(dbOptions.ConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.StartsWith("PersistanceDbUp.Scripts."))
                    .WithTransaction()
                    .LogToConsole()
                    .Build();

                var result = upgrader.PerformUpgrade();

                if (result.Successful)
                {
                    Console.WriteLine();
                    Console.WriteLine("Migracja bazy danych zakończona pomyślnie!");
                    
                    // Pokaż historię migracji
                    await ShowMigrationHistoryAsync(dbOptions.ConnectionString);
                    
                    Console.WriteLine();
                    Console.WriteLine("Aby wygenerować dane testowe, użyj klasy DataGenerator.");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Błąd podczas migracji:");
                    Console.WriteLine(result.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Wystąpił błąd:");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Szczegóły:");
                    Console.WriteLine(ex.InnerException.Message);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        private static async Task EnsureDatabaseExistsAsync(string connectionString, string databaseName)
        {
            try
            {
                // Utwórz connection string do master database
                var masterConnectionString = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "master"
                }.ConnectionString;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(masterConnectionString);
                await connection.OpenAsync();

                // Sprawdź czy baza danych istnieje
                var checkDatabaseQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";
                using var checkCommand = new Microsoft.Data.SqlClient.SqlCommand(checkDatabaseQuery, connection);
                var result = await checkCommand.ExecuteScalarAsync();
                var databaseExists = result != null && (int)result > 0;

                if (!databaseExists)
                {
                    Console.WriteLine($"Baza danych '{databaseName}' nie istnieje.");
                    throw new InvalidOperationException($"Baza danych '{databaseName}' nie została znaleziona. Upewnij się, że jest poprawnie skonfigurowana w pliku konfiguracyjnym i czy itnieje faktycznie na serwerze.");
                }
                else
                {
                    Console.WriteLine($"Baza danych '{databaseName}' już istnieje.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas sprawdzania bazy danych: {ex.Message}");
                throw;
            }
        }

        private static async Task ShowMigrationHistoryAsync(string connectionString)
        {
            try
            {
                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                // Sprawdź czy tabela SchemaVersions istnieje
                var checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SchemaVersions'";
                using var checkCommand = new Microsoft.Data.SqlClient.SqlCommand(checkTableQuery, connection);
                var result = await checkCommand.ExecuteScalarAsync();
                var tableExists = result != null && (int)result > 0;

                if (tableExists)
                {
                    Console.WriteLine();
                    Console.WriteLine("=== Historia migracji ===");
                    
                    var historyQuery = "SELECT ScriptName, Applied FROM SchemaVersions ORDER BY Applied";
                    using var historyCommand = new Microsoft.Data.SqlClient.SqlCommand(historyQuery, connection);
                    using var reader = await historyCommand.ExecuteReaderAsync();
                    
                    while (await reader.ReadAsync())
                    {
                        var scriptName = reader["ScriptName"].ToString();
                        var applied = reader.GetDateTime(reader.GetOrdinal("Applied"));
                        Console.WriteLine($"📄 {scriptName} - {applied:yyyy-MM-dd HH:mm:ss}");
                    }
                }
                else
                {
                    Console.WriteLine("Tabela SchemaVersions nie istnieje - żadne migracje nie zostały jeszcze wykonane.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas sprawdzania historii migracji: {ex.Message}");
            }
        }
    }
}
