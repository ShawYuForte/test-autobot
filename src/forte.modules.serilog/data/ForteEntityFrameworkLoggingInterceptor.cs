using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using forte.services;

namespace forte.data
{
    public class ForteEntityFrameworkLoggingInterceptor : IDbCommandInterceptor
    {
        private readonly ILogger _logger;

        public ForteEntityFrameworkLoggingInterceptor(ILogger logger)
        {
            _logger = logger;
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogExecutingCommand(command);
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogExecutedCommand(command);
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            LogExecutingCommand(command);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            LogExecutedCommand(command);
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogExecutingCommand(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogExecutedCommand(command);
        }

        private static void LogExecutedCommand(DbCommand command)
        {
            // No need to log it as if no following errors logged all was done OK.
        }

        private void LogExecutingCommand(DbCommand command)
        {
            var query = command?.CommandText;
            var database = command?.Connection?.Database;
            var server = command?.Connection?.DataSource;

            _logger.Debug("{@database} on {@server}: {@query}", database, server, query);
        }
    }
}
