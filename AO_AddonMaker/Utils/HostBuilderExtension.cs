using Addon.Files;
using Application.ViewModels;
using Application.Views;
using Autofac;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;

namespace Application.Utils
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureNLog(this IHostBuilder hostBuilder)
        {
            var target = new MethodCallTarget(nameof(App.Log), App.Log);
            SimpleConfigurator.ConfigureForTargetLogging(target, NLog.LogLevel.Debug);
            hostBuilder.ConfigureLogging(logging => logging.AddNLog());

            return hostBuilder;
        }
        public static IHostBuilder ConfigureAppContainer(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterType<FileManager>()
                    .As<IFileManager>()
                    .SingleInstance();
                builder.RegisterType<Project>()
                    .AsSelf();
                builder.RegisterType<MainWindow>()
                    .AsSelf();
                builder.RegisterType<MainWindowViewModel>()
                    .AsSelf();

                builder.RegisterType<LoggerFactory>()
                    .As<ILoggerFactory>()
                    .SingleInstance();
                builder.RegisterGeneric(typeof(Logger<>))
                    .As(typeof(ILogger<>))
                    .SingleInstance();
            });
            return hostBuilder;
        }
    }
}