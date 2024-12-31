using System;
using System.IO;

public static class ErrorLogger
{
    private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "error_log.txt");

    static ErrorLogger()
    {
        // Ensure the Logs folder exists
        var logDirectory = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    public static void LogError(Exception ex)
    {
        try
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error: {ex.Message}\nStack Trace: {ex.StackTrace}\n";

            // Write the error message to the log file
            File.AppendAllText(logFilePath, logMessage);
        }
        catch (Exception loggingEx)
        {
            // If an error occurs while logging, handle it here (you could log this exception somewhere else)
            File.AppendAllText(logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Logging Error: {loggingEx.Message}\n");
        }
    }
}
