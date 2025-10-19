using System.Data.Common;
using Npgsql;

namespace backend.Database
{
    public class MigrationRunner
    {
        private readonly DbConnectionFactory _factory;
        private readonly string _migrationsFolder;

        public MigrationRunner(DbConnectionFactory factory, string migrationsFolder = "Database/Migrations")
        {
            _factory = factory;
            _migrationsFolder = migrationsFolder;
        }
        public async Task ApplyMigrationsAsync()
        {
            await using var conn = _factory.CreateConnection();
            await EnsureMigrationSchema(conn);
            HashSet<string> appliedMigrations = await ReadAppliedMigrations(conn);
            var migrations = Directory.GetFiles(_migrationsFolder, "*.sql").OrderBy(f => f).ToList();
            await ApplyNewMigrations(migrations, appliedMigrations, conn);

        }

        private async Task EnsureMigrationSchema(NpgsqlConnection conn)
        {
            var migrationSchemaFile = Path.Combine(_migrationsFolder, "001_create_schema_migrations.sql");
            var migrationSchemaSql = await File.ReadAllTextAsync(migrationSchemaFile) ?? throw new InvalidOperationException("No schema migrations sql file");
            await using var cmd = new NpgsqlCommand(migrationSchemaSql, conn);
            await cmd.ExecuteNonQueryAsync();
        }
        private async Task<HashSet<string>> ReadAppliedMigrations(NpgsqlConnection conn)
        {
            var appliedFiles = new HashSet<string>();
            var getAppliedSql = "SELECT filename FROM schema_migrations;";
            await using (var cmd = new NpgsqlCommand(getAppliedSql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                    appliedFiles.Add(reader.GetString(0));

            return appliedFiles;
        }
        private async Task ApplyNewMigrations(List<string> migrations, HashSet<string> appliedMigrations, NpgsqlConnection conn)
        {
            foreach (var migration in migrations)
            {
                var name = Path.GetFileName(migration);
                if (appliedMigrations.Contains(name)) continue;

                var sql = await File.ReadAllTextAsync(migration);
                await using (var cmd = new NpgsqlCommand(sql, conn))
                    await cmd.ExecuteNonQueryAsync();

                var recordSql = "INSERT INTO schema_migrations (filename) VALUES (@mergeName)";
                await using (var cmd = new NpgsqlCommand(recordSql, conn))
                {
                    cmd.Parameters.AddWithValue("mergeName", migration);
                    await cmd.ExecuteNonQueryAsync();
                }


            }   
        }
    }
}