namespace discovery_service.domain.Entity
{
    /*
        Basic health information of a service instance
    */
    public class ServerHealth
    {
        public string ServiceId { get; private set; }

        public HealthStatusEnum Status { get; private set; }

        public DateTime LastChecked { get; private set; }

        public string Message { get; private set; }         /* Optional message (e.g., error or OK) */


        public ServerHealth(string serviceId, HealthStatusEnum status, string message)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("ServiceId cannot be empty");

            ServiceId = serviceId;
            Status = status;
            Message = message;
            LastChecked = DateTime.UtcNow;
        }

        /* Update health info */
        public void Update(HealthStatusEnum newStatus, string message)
        {
            Status = newStatus;
            Message = message;
            LastChecked = DateTime.UtcNow;
        }
    }

    /*
        Enum for health state
    */
    public enum HealthStatusEnum
    {
        Unknown,
        Passing,
        Warning,
        Critical
    }
}
