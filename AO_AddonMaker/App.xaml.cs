using System;
using System.Windows;
using System.Windows.Navigation;
using Application.Utils;
using Application.Views;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Application
{
    public partial class App : System.Windows.Application
    {
        public static event Action<LogEventInfo, object[]> OnLogHandler;

        private IHost Host { get; set; }

        public App()
        {
            Host = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppContainer()
                .ConfigureNLog()
                .Build();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await Host.StartAsync();

            var model = Host.Services.GetAutofacRoot().Resolve<MainWindowViewModel>();
            var view = new MainWindow
            {
                DataContext = model
            };
            view.Show();
        }

        protected async override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            await Host.StopAsync();
        }

        public static void Log(LogEventInfo logEventInfo, object[] objects)
        {
            OnLogHandler?.Invoke(logEventInfo, objects);
        }
    }
}