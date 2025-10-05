namespace discovery_service.domain.Entity
{
    /**
     * Basic service information in discovery domain
     */
    public class ServiceInfo_
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        /** Host/IP address */
        public string Address { get; private set; }

        public int Port { get; private set; }

        public IReadOnlyList<string> Tags { get; private set; }  /** Optional tags (e.g., v1, region=asia) */


        public ServiceStatus Status { get; private set; }

        /** Constructor */
        public ServiceInfo_(string id, string name, string address, int port, IEnumerable<string>? tags = null)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be null or empty");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty");
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address cannot be null or empty");
            if (port <= 0) throw new ArgumentException("Port must be positive");

            Id = id;
            Name = name;
            Address = address;
            Port = port;
            Tags = tags?.ToList() ?? new List<string>();
            Status = ServiceStatus.Unknown;
        }

        /** Update health status */
        public void UpdateStatus(ServiceStatus newStatus) => Status = newStatus;

        public string GetEndpoint() => $"{Address}:{Port}";   /** Return full endpoint (e.g., 127.0.0.1:5000) */

    }

    /** Enum for service health status */
    public enum ServiceStatus
    {
        Unknown,
        Healthy,
        Unhealthy
    }
}
