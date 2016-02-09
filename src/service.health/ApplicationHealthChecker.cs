namespace service.health
{
    using lib.logging;

    public interface IApplicationHealthChecker
    {
        bool IsHealthy();
    }

    internal sealed class ApplicationHealthChecker : IApplicationHealthChecker
    {
        readonly ILogProvider log;

        public ApplicationHealthChecker(ILogProvider log)
        {
            this.log = log;
        }

        public bool IsHealthy()
        {
            //TODO: Implement Health Check
            return true;
        }
    }
}
