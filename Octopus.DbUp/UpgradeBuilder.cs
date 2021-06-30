using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Octopus.DbUp
{
    public class UpgraderBuilder
    {
        private ILogger _logger;
        private string _connectionString;
        private readonly string _dbEngineType;
        private readonly int _dbUpTimeout;
        private Action<Upgrader> _setScriptsToRun = upgrader => { };

        public UpgraderBuilder()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "appsettings.json";
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path, optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var settings = new AppSettings.AppSettings();
            configuration.Bind(settings);

            _connectionString = settings.DbUpDataSource;
            _dbEngineType = settings.DbEngineType;
            _dbUpTimeout = settings.DbUpTimeout;
        }

        public UpgraderBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public UpgraderBuilder WithPreScriptsToRunEverytime()
        {
            _setScriptsToRun = upgrader => upgrader.SelectPreScriptsRunningEverytime();
            return this;
        }

        public UpgraderBuilder WithPostScriptsToRunEverytime()
        {
            _setScriptsToRun = upgrader => upgrader.SelectPostScriptsRunningEverytime();
            return this;
        }

        public UpgraderBuilder WithPreDeploymentScripts()
        {
            _setScriptsToRun = upgrader => upgrader.SelectPreDeploymentScripts();
            return this;
        }

        public UpgraderBuilder WithDeploymentScripts()
        {
            _setScriptsToRun = upgrader => upgrader.SelectDeploymentScripts();
            return this;
        }

        public UpgraderBuilder WithPostDeploymentScripts()
        {
            _setScriptsToRun = upgrader => upgrader.SelectPostDeploymentScripts();
            return this;
        }

        public UpgraderBuilder WithConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public Upgrader Build()
        {
            var upgrader = new Upgrader(_connectionString, _dbEngineType, _logger, _dbUpTimeout);
            _setScriptsToRun(upgrader);

            return upgrader;
        }
    }
}
