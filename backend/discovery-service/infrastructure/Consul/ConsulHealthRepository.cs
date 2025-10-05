using Consul;
using discovery_service.domain.Repository;
using discovery_service.domain.Entity;
/*
    Implements IHealthRepository using Consul as the backend
*/
namespace discovery_service.infrastructure.Consul
{
    public class ConsulHealthRepository : IHealthRepository
    {
        private readonly IConsulClient _consulClient;
        public ConsulHealthRepository(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public ServerHealth? GetHealthInfo(ServerHealth health)
        {
            /*
                Using Agent Service Check for both health status and metadata
            */

            try
            {
                var checks = _consulClient.Health.Checks(health.ServiceId).Result.Response.FirstOrDefault();
                if (checks == null)
                    return null;
                var status = checks.Status.ToString() switch
                {
                    "Passing" => HealthStatusEnum.Passing,
                    "Warning" => HealthStatusEnum.Warning,
                    "Critical" => HealthStatusEnum.Critical,
                    _ => HealthStatusEnum.Unknown
                };
                return new ServerHealth(
                    health.ServiceId,
                    status,
                    checks.Output ?? "No message"
                );
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching health info for service {health.ServiceId}: {e.Message}");
            }
        }
        
        public List<ServerHealth> GetAllHealth (string serviceName)
        {
            List<ServerHealth> healthList = new List<ServerHealth>();
            try
            {
                var checks = _consulClient.Health.Service(serviceName).Result.Response;
                foreach (var check in checks)
                {
                    var status = check.Checks[0].Status.ToString() switch
                    {
                        "Passing" => HealthStatusEnum.Passing,
                        "Warning" => HealthStatusEnum.Warning,
                        "Critical" => HealthStatusEnum.Critical,
                        _ => HealthStatusEnum.Unknown
                    };
                    healthList.Add(new ServerHealth(
                        check.Service.ID,
                        status,
                        check.Checks[0].Output ?? "No message"
                    ));
                }
                return healthList;
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching health info for service {serviceName}: {e.Message}");
            }
        }

    }
}