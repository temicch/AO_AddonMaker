using System;
using System.Windows;
using Addon.Files;
using AO_AddonMaker.Views;
using Autofac;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using LogLevel = NLog.LogLevel;

namespace AO_AddonMaker
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static event Action<LogEventInfo, object[]> OnLogHandler;

        protected override void OnStartup(StartupEventArgs e)
        {
            var container = CreateContainer();

            ConfigureNLog(container);

            var model = container.Resolve<MainWindowViewModel>();
            var view = new MainWindow
            {
                DataContext = model
            };
            view.Show();
        }

        private void ConfigureNLog(IContainer container)
        {
            var target = new MethodCallTarget(nameof(Log), Log);
            SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
            var loggerFactory = container.Resolve<ILoggerFactory>();
            loggerFactory.AddNLog();
        }

        private static void Log(LogEventInfo logEventInfo, object[] objects)
        {
            OnLogHandler?.Invoke(logEventInfo, objects);
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

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

            return builder.Build();
        }
    }
}