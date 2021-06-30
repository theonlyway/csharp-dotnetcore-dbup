using System;

namespace Octopus.DbUp
{
    internal static class Program
    {
        public static int Main()
        {
            var logger = new ConsoleLogger();
            logger.Info("Started upgrading database.");

            foreach (var item in typeof(Constants.ScriptFolder).GetAllPublicConstantValues<string>())
            {
                System.IO.Directory.CreateDirectory(item);
            }

            // See "ReadMe about scripts.txt" about how this application works/intended to work
            var runnerWithPreScriptsToRunEveryTime =
                new UpgraderBuilder()
                    .WithLogger(logger)
                    .WithPreScriptsToRunEverytime()
                    .Build();
            var runnerWithPreDeploymentScripts =
                new UpgraderBuilder()
                    .WithLogger(logger)
                    .WithPreDeploymentScripts()
                    .Build();
            var runnerWithDeploymentScripts =
                new UpgraderBuilder()
                    .WithLogger(logger)
                    .WithDeploymentScripts()
                    .Build();
            var runnerWithPostDeploymentScripts =
                new UpgraderBuilder()
                    .WithLogger(logger)
                    .WithPostDeploymentScripts()
                    .Build();
            var runnerWithPostScriptsToRunEveryTime =
                new UpgraderBuilder()
                    .WithLogger(logger)
                    .WithPostScriptsToRunEverytime()
                    .Build();
            var upgradeSuccessful =
                runnerWithPreScriptsToRunEveryTime.Run() && runnerWithPreDeploymentScripts.Run() &&
                runnerWithDeploymentScripts.Run() && runnerWithPostDeploymentScripts.Run() && runnerWithPostScriptsToRunEveryTime.Run();

            logger.Info(Constants.Separator);
            logger.Info("Upgrade database finished.");

#if DEBUG
            Console.ReadKey();
#endif

            return upgradeSuccessful ? 0 : -1;
        }
    }
}
