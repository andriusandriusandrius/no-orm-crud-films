using backend.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddSingleton<MigrationRunner>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
    await migrationRunner.ApplyMigrationsAsync();
}


    app.Run();
