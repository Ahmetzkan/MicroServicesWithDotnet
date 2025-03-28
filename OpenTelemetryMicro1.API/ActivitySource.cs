using System.Diagnostics;

namespace OpenTelemetryMicro1.API
{
    public class ActivitySourceProvider
    {
        public static ActivitySource Instance = new ActivitySource("ActivitySourceProvider", "v1");
    }
}