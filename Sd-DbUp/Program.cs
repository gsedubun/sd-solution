// See https://aka.ms/new-console-template for more information

using DbUp;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var connectionString =
    config.GetConnectionString("DefaultConnection");

// Ensure database is created
EnsureDatabase.For.SqlDatabase(connectionString);

var upgrader =
    DbUp.DeployChanges.To
        .SqlDatabase(connectionString)
        .WithScriptsFromFileSystem("Scripts")
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();
if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
    return -1;
}
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
Console.ReadKey();
return 0;
