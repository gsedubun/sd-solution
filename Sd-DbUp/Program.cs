// See https://aka.ms/new-console-template for more information

using DbUp;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var salesConnectionString =
    config.GetConnectionString("SalesConnection");

// Ensure database is created
EnsureDatabase.For.SqlDatabase(salesConnectionString);

var upgrader =
    DbUp.DeployChanges.To
        .SqlDatabase(salesConnectionString)
        .WithScriptsFromFileSystem("Sales_Scripts")
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
Console.WriteLine("Sales Database Upgrade Success!");
Console.ResetColor();





var storesConnectionString =
    config.GetConnectionString("StoresConnection");

// Ensure database is created
EnsureDatabase.For.SqlDatabase(storesConnectionString);

var storesUpgrader =
    DbUp.DeployChanges.To
        .SqlDatabase(storesConnectionString)
        .WithScriptsFromFileSystem("Stores_Scripts")
        .LogToConsole()
        .Build();

var resultStores = storesUpgrader.PerformUpgrade();
if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
    return -1;
}
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Stores Database Upgrade Success!");
Console.ResetColor();

return 0;
