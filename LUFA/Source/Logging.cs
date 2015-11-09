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

        public static void Log(Severity severity, string message, params object[] formatArgs)
        {
            switch (severity)
            {
                case Severity.Error:
                    ActivityLog.LogError(LogSourceName, string.Format(message, formatArgs));
                    break;
                case Severity.Information:
                    ActivityLog.LogInformation(LogSourceName, string.Format(message, formatArgs));
                    break;
                case Severity.Warning:
                    ActivityLog.LogWarning(LogSourceName, string.Format(message, formatArgs));
                    break;
            }
        }
    }
}
