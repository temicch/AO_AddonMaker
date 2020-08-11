using Application.Utils;
using Application.ViewModels;
using Application.Views;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Windows;

namespace Application
{
    public partial class App
    {
        public static event Action<LogEventInfo, object[]> OnLogHandler;

        private IHost Host { get; }

        public App()
        {
            Host = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppContainer()
                .ConfigureNLog()
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
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

        protected override async void OnExit(ExitEventArgs e)
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