using Addon.Files;
using AO_AddonMaker.Views;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Windows;

namespace AO_AddonMaker
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IContainer container = CreateContainer();

            var loggerFactory = container.Resolve<ILoggerFactory>();
            loggerFactory.AddNLog();
            //var nlogConfig = new LoggingConfiguration();
            //var traceTarget = new TraceTarget("trace")
            //{
            //    Layout =
            //        "${message} ${exception}"
            //};
            //nlogConfig.AddTarget(traceTarget);
            //nlogConfig.AddRuleForAllLevels(traceTarget, "*");
            //LogManager.Configuration = nlogConfig;

            var model = container.Resolve<MainWindowViewModel>();
            var view = new MainWindow 
            { 
                DataContext = model 
            };
            view.Show();
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