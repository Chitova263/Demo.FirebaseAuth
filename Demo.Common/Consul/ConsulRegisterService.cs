using Consul;
using Demo.Api.Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo.Common.Consul;
public class ConsulRegisterService: IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly ILogger<ConsulRegisterService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ConsulConfiguration _consulConfiguration;

    public ConsulRegisterService(
        IConsulClient consulClient,
        IOptions<ConsulConfiguration> consulConfigurationOptions,
        ILogger<ConsulRegisterService> logger, 
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _consulClient = consulClient ?? throw new ArgumentNullException(nameof(consulClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _consulConfiguration = consulConfigurationOptions.Value ?? throw new ArgumentNullException(nameof(consulConfigurationOptions));
        
        _logger.LogInformation($"######################ConsulRegisterService is created {_consulConfiguration.ServiceName}");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogWarning("**************************************************************************");
        
        // deregister service on shutdown
        await _consulClient.Agent.ServiceDeregister(_consulConfiguration.ServiceId, cancellationToken);
    
        _logger.LogInformation($"Registering service ID:  {_consulConfiguration.ServiceId}");
        var uri = new Uri(_consulConfiguration.Address);
        var registration = new AgentServiceRegistration()
        {
            ID = _consulConfiguration.ServiceId,
            Name = _consulConfiguration.ServiceName,
            Address = uri.Host,
            Port = uri.Port,
            Tags = new[] { _consulConfiguration.ServiceName },
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
        await _consulClient.Agent.ServiceDeregister(_consulConfiguration.ServiceId, cancellationToken);
    }
}