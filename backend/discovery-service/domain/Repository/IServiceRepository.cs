using discovery_service.domain.Entity;

namespace discovery_service.domain.Repository
{
    /*
        Repository abstraction for managing services
    */
    public interface IServiceRepository
    {
        /* Register a new service */
        Task Register(ServiceInfo_ service);

        /* Deregister a service by Id */
        Task Deregister(string serviceId);

        /* Get all instances of a given service */
        List<ServiceInfo_> GetAllServices(string serviceName);

        /* Get a single best instance of a service  */
        Task <ServiceInfo_?> GetBestInstance(string serviceName);
    }
}
