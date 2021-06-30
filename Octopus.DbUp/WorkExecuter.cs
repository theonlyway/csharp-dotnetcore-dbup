using System;
using System.Diagnostics;

namespace Octopus.DbUp
{
    internal class WorkExecuter
    {
        private readonly ILogger _logger;

        public WorkExecuter(ILogger logger)
        {
            _logger = logger;
        }

        public T Run<T>(Func<T> work, T errorResult)
        {
            var timer = new Stopwatch();

            try
            {
                timer.Start();
                return work();
            }
            catch (Exception ex)
            {
                _logger?.Exception(ex);
                return errorResult;
            }
            finally
            {
                timer.Stop();
                _logger?.Info($"Execution took {timer.ElapsedMilliseconds}ms.");
            }
        }
    }
}
