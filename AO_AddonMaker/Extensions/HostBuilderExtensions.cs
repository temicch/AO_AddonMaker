using Application.BL;
using Application.BL.Files;
using Application.PL.ViewModels;
using Application.PL.Views;
using Autofac;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using LogLevel = NLog.LogLevel;

namespace Application.PL.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureNLog(this IHostBuilder hostBuilder)
        {
            var target = new MethodCallTarget(nameof(App.Log), App.Log);
            SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
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