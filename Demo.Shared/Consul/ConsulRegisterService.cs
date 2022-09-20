using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo.Shared.Consul;
public class ConsulRegisterService: IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly IOptions<ConsulConfiguration> _consulConfiguration;
    private readonly ILogger<ConsulRegisterService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public ConsulRegisterService(
        IConsulClient consulClient,
        IOptions<ConsulConfiguration> consulConfiguration,
        ILogger<ConsulRegisterService> logger, 
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _consulClient = consulClient ?? throw new ArgumentNullException(nameof(consulClient));
        _consulConfiguration = consulConfiguration ?? throw new ArgumentNullException(nameof(consulConfiguration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // deregister service on shutdown
        await _consulClient.Agent.ServiceDeregister(_consulConfiguration.Value.ServiceId, cancellationToken);
    
        _logger.LogInformation("Registering service with Consul");
        var uri = new Uri(_consulConfiguration.Value.Address);
        var registration = new AgentServiceRegistration()
        {
            ID = _consulConfiguration.Value.ServiceId,
            Name = _consulConfiguration.Value.ServiceName,
            Address = uri.Host,
            Port = uri.Port,
            Tags = new[] { "IdentityService" },
            // Check = new AgentServiceCheck()
            // {
            //     DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
            //     Interval = TimeSpan.FromSeconds(10),
            //     HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/health",
            //     Timeout = TimeSpan.FromSeconds(5)
            // }
        };
        
        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

        _hostApplicationLifetime.ApplicationStopping.Register(() =>
        {
            _logger.LogInformation("De-registering service from Consul");
            _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken).GetAwaiter().GetResult();
        });
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _consulClient.Agent.ServiceDeregister(_consulConfiguration.Value.ServiceId, cancellationToken);
    }
}