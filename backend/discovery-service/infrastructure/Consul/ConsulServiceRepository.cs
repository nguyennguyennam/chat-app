using Consul;
using discovery_service.domain.Repository;
using discovery_service.domain.Entity;
using discovery_service.infrastructure.Consul;

namespace discovery_service.infrastructure.Consul
{
    public class ConsulServiceRepository : IServiceRepository
    {
        /*
            This class implements IServiceRepository, including service registration, get services and server selection.
        */
        private readonly IConsulClient _consul;
        private readonly IHealthRepository _healthRepository; 
        public ConsulServiceRepository(IConsulClient consul, IHealthRepository healthRepository)
        {
            _consul = consul;
            _healthRepository = healthRepository;
        }
        /*
            Register a new service
        */
        public async Task Register(ServiceInfo_ service)
        {
            try
            {
                // Define service registration details
                var registration = new AgentServiceRegistration()
                {
                    ID = service.Id,
                    Name = service.Name,
                    Address = service.Address,
                    Port = service.Port,
                    Tags = service.Tags?.ToArray() ?? Array.Empty<string>(),

                    // Optional: HTTP health check (recommended)
                    Check = new AgentServiceCheck()
                    {
                        HTTP = $"http://{service.Address}:{service.Port}/health",
                        Interval = TimeSpan.FromSeconds(10),           // how often Consul checks
                        Timeout = TimeSpan.FromSeconds(3),             // request timeout
                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
                    }
                };

                // Register with Consul
                await _consul.Agent.ServiceRegister(registration);

                Console.WriteLine($"✅ Registered service '{service.Name}' ({service.Id}) at {service.Address}:{service.Port}");
            }
            catch (Exception ex)
            {
                throw new Exception($" Failed to register service '{service.Name}': {ex.Message}", ex);
            }

            /*
                This function Deregister a service by its ID
            */
            public async Task Deregister(string serviceId)
        {
            await _consul.Agent.ServiceDeregister(serviceId);
        }

        /*
            Get all instances of a given service
        */
        public List<ServiceInfo_> GetAllServices(string serviceName)
        {
            var services = _consul.Agent.Services().Result.Response;
            var result = new List<ServiceInfo_>();
            foreach (var service in services.Values)
            {
                if (service.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(new ServiceInfo_(
                        service.ID,
                        service.Service,
                        service.Address,
                        service.Port,
                        service.Tags ?? Array.Empty<string>()
                    ));
                }
            }
            return result;
        }
        
        /*
            Select the best instance of a service.
        */

        public async Task<ServiceInfo_?> GetBestInstance (string serviceName)
        {
            List<ServerHealth> serviceHealths = _healthRepository.GetAllHealth(serviceName);
            if (serviceHealths.Count == 0)
            {
                throw new Exception($"No instances found for service '{serviceName}'");
            }
            // Filter healthy instances
            var healthyInstance = serviceHealths.Where(h =>
                h.Status == HealthStatusEnum.Passing)
                .ToList();
            if (healthyInstance.Count == 0)
            {
                throw new Exception($"No healthy instances found for service '{serviceName}'");
            }
            //Choose the best one by using Least Connection Strategy.
            
        }
    }
}
