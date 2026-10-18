module OpenPayments.Sandbox.Infrastructure.Database.DatabaseUpgrader

open System.Reflection
open System.Text.RegularExpressions
open DbUp
open DbUp.Builder
open DbUp.Helpers
open Microsoft.Extensions.Logging

let private importerPattern  = Regex(@"^Importers\.[a-z0-9_]+\.sql$", RegexOptions.Compiled)
let private migrationPattern = Regex(@"^Migrations\.\d{8}_\d{4}_[a-z0-9_]+\.sql$", RegexOptions.Compiled)

let private run (loggerFactory: ILoggerFactory) (builder: UpgradeEngineBuilder) =
    let result = builder.LogTo(loggerFactory).Build().PerformUpgrade()
    if not result.Successful then raise result.Error

let upgrade (loggerFactory: ILoggerFactory) (connectionString: string) =
    let assembly = Assembly.GetExecutingAssembly()

    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(assembly, fun name -> migrationPattern.IsMatch name)
        .WithTransactionPerScript()
    |> run loggerFactory

    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(assembly, fun name -> importerPattern.IsMatch name)
        .JournalTo(NullJournal())
    |> run loggerFactory
