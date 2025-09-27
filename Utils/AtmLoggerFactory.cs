using Microsoft.Extensions.Logging;
using Serilog;

namespace BankingApplication.Utils
{
    public static class AtmLoggerFactory
    {
        private static readonly ILoggerFactory loggerFactory;
        //private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
        //    "COMM_Back-end_C#\\COMM_Back-end_C#_FirstProject\\BankingApplication\\Logs\\logs.log");
        private static readonly string filePath = Utils.GetFilePathFromProject("Logs", "logs.log");

        static AtmLoggerFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                // .WriteTo.Console()
                .WriteTo.File(filePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
                .CreateLogger();

            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });
        }

        public static ILogger<T> CreateLogger<T>() => loggerFactory.CreateLogger<T>();
    }
}