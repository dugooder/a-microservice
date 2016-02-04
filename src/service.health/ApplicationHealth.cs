namespace service.health
{
    using lib.logging;
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

        public string Status
        {
            get
            {
                return this.IsHealthy() ? "Bien" : "Muy mal";

            }
        }
    }
}
