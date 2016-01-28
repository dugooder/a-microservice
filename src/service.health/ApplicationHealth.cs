using common;

namespace service.health
{
    public class ApplicationHealth
    {
        readonly ILogProvider log;

        public ApplicationHealth(ILogProvider log)
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
