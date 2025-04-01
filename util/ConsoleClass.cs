namespace LeadwaycanteenApi.util
{
    /// <summary>
    /// Logger utill for logging
    /// </summary>
    /// <typeparam name="T">Logger Type</typeparam>
    public static class ConsoleClass<T>
    {
        internal static ILoggerFactory LoggerFactory { get; set; } = new LoggerFactory();
        internal readonly static ILogger _logger = LoggerFactory.CreateLogger<T>();

        /// <summary>
        /// Logger utill for logging Errors
        /// </summary>
        /// <param name="ex">The exception that caused the error</param>
        /// <param name="message">The exception message</param>
        public static void LogError(Exception ex, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            _logger.LogError(ex, message);
            Console.ResetColor();
        }

        /// <summary>
        /// Logging information
        /// </summary>
        /// <param name="message">The message to be logged</param>
        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            _logger.LogInformation(message);
            Console.ResetColor();
        }
    }
}
