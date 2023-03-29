using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application;
using Application.Interfaces;
using Infrastructure;
using Serilog;

namespace ConsoleInterface;

public class Program
{
    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json",
                optional: true)
            .AddEnvironmentVariables();
    }

    public static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.File("/var/log/system-monitor/log", rollOnFileSizeLimit: true)
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger.Information("Application Starting");

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IMonitoringService, MonitoringService>();
                services.AddSingleton<ITemperatureMonitor, TemperatureMonitor>();
                services.AddTransient<IDeviceExplorer, DeviceExplorer>();
            })
            .UseSerilog()
            .Build();

        var deviceExplorer = ActivatorUtilities.CreateInstance<DeviceExplorer>(host.Services);
        deviceExplorer.Run();
    }
}