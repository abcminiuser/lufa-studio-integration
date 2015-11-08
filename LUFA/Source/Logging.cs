using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    public abstract class Logging
    {
        public enum Severity
        {
            Information,
            Warning,
            Error,
        }

        private static string LogSourceName = "LUFA Library Extension";

        public static void Log(Severity severity, string message)
        {
            switch (severity)
            {
                case Severity.Error:
                    ActivityLog.LogError(LogSourceName, message);
                    break;
                case Severity.Information:
                    ActivityLog.LogInformation(LogSourceName, message);
                    break;
                case Severity.Warning:
                    ActivityLog.LogWarning(LogSourceName, message);
                    break;
            }
        }
    }
}
