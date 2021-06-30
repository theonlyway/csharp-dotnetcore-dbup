using System;
using DbUp;
using DbUp.Builder;
using DbUp.Helpers;
using DbUp.ScriptProviders;
using DbUp.Oracle;

namespace Octopus.DbUp
{
    public class Upgrader
    {
        private readonly string _connectionString;
        private readonly string _dbEngineType;
        private readonly int _dbUpTimeout;
        private readonly ILogger _logger;
        private Func<string, bool> _scriptSearchFunc = s => true;
        private string _folder = string.Empty;
        private Action<UpgradeEngineBuilder> _journalingSetup = u => { };
        private UpgradeEngineBuilder upgradeBuilder;

        public Upgrader(string connectionString, string dbEngineType, ILogger logger, int dbUpTimeout)
        {
            _connectionString = connectionString;
            _logger = logger;
            _dbEngineType = dbEngineType;
            _dbUpTimeout = dbUpTimeout;
        }

        public bool Run()
        {
            return
                new WorkExecuter(_logger)
                    .Run(() =>
                    {
                        _logger?.Info(Constants.Separator);
                        _logger?.Info($"To run scripts from folder '{_folder}'.");
                        switch (this._dbEngineType.ToLower())
                        {
                            case "mssql":
                                _logger?.Info($"DB engine type set to: '{_dbEngineType}'");
                                upgradeBuilder = DeployChanges.To.SqlDatabase(_connectionString);
                                EnsureDatabase.For.SqlDatabase(_connectionString);
                                break;
                            case "postgres":
                                _logger?.Info($"DB engine type set to: '{_dbEngineType}'");
                                upgradeBuilder = DeployChanges.To.PostgresqlDatabase(_connectionString);
                                EnsureDatabase.For.PostgresqlDatabase(_connectionString);
                                break;
                            case "mysql":
                                _logger?.Info($"DB engine type set to: '{_dbEngineType}'");
                                upgradeBuilder = DeployChanges.To.MySqlDatabase(_connectionString);
                                EnsureDatabase.For.MySqlDatabase(_connectionString);
                                break;
                            case "redshift":
                                _logger?.Info($"DB engine type set to: '{_dbEngineType}'");
                                upgradeBuilder = DeployChanges.To.RedshiftDatabase(_connectionString);
                                EnsureDatabase.For.RedshiftDatabase(_connectionString);
                                break;
                            case "oracle":
                                _logger?.Info($"DB engine type set to: '{_dbEngineType}'");
                                upgradeBuilder = DeployChanges.To.OracleDatabase(_connectionString);
                                break;
                            default:
                                throw new ArgumentException($"Unknown DB engine: '{_dbEngineType}'");
                        }
                        _journalingSetup(upgradeBuilder);
                        FileSystemScriptOptions fsso = new FileSystemScriptOptions
                        {
                            IncludeSubDirectories = true,
                            Extensions = new string[] {"*.sql"}
                        };
                        var path = AppDomain.CurrentDomain.BaseDirectory + _folder;
                        var upgrader =
                            upgradeBuilder
                                .WithScriptsFromFileSystem(path, fsso)
                                .LogScriptOutput()  // Log out from the script itself
                                .LogToConsole()     // Log dbUp output
                                .WithTransaction()  // Run all the scripts in one transation..
                                .WithExecutionTimeout(TimeSpan.FromSeconds(_dbUpTimeout))
                                .Build();
                        var runnerResult = upgrader.PerformUpgrade();

                        if (runnerResult.Successful)
                        {
                            _logger?.Success("Update run completed!");
                        }
                        else
                        {
                            _logger?.Exception(runnerResult.Error);
                        }
                        return runnerResult.Successful;
                    },
                    false);
        }

        public void SelectPreScriptsRunningEverytime()
        {
            DoNotRecordScriptExecution();
            UseScriptsInFolder(Constants.ScriptFolder.RunAlwaysPreScripts);
        }

        public void SelectPostScriptsRunningEverytime()
        {
            DoNotRecordScriptExecution();
            UseScriptsInFolder(Constants.ScriptFolder.RunAlwaysPostScripts);
        }

        public void SelectPreDeploymentScripts()
        {
            UseScriptsInFolder(Constants.ScriptFolder.PreDeploymentScripts);
        }

        public void SelectDeploymentScripts()
        {
            UseScriptsInFolder(Constants.ScriptFolder.DeploymentScripts);
        }

        public void SelectPostDeploymentScripts()
        {
            UseScriptsInFolder(Constants.ScriptFolder.PostDeploymentScripts);
        }

        private void DoNotRecordScriptExecution()
        {
            // By Journalling to nothing, the schema table is not written to and will execute everytime
            _journalingSetup = upgrader => upgrader.JournalTo(new NullJournal());
        }

        private void UseScriptsInFolder(string folderName)
        {
            // Note: 
            // - SQL script need to be include in the project as an embedded resource in the folder
            // - Scripts will be run alpha numberically, which includes the folder names, so multiple folders where the 
            //      date is included will ensure that they run in correct order
            _folder = folderName;
            _scriptSearchFunc = s => s.Contains(folderName);
        }
    }
}
