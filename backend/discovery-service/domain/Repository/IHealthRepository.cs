using discovery_service.domain.Entity;

namespace discovery_service.domain.Repository
{
    /*
        Repository abstraction for managing health info of services
    */
    public interface IHealthRepository
    {
        /* Get health info of a specific service instance */
        public ServerHealth? GetHealthInfo(ServerHealth health);


        /* Get health info of all instances in a service */
        public List<ServerHealth> GetAllHealth(string serviceName);
    }
}
