using Application.PL.Extensions;
using Application.PL.ViewModels;
using Application.PL.Views;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Windows;

namespace Application.PL
{
    public partial class App
    {
        public App()
        {
            Host = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppContainer()
                .ConfigureNLog()
                .Build();
        }

        private IHost Host { get; }
        public static event Action<LogEventInfo, object[]> OnLogHandler;

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
            using (Host)
            {
                await Host.StopAsync();
            }
        }

        /// <summary>
        /// Invoke log method which can be listened
        /// </summary>
        /// <param name="logEventInfo"></param>
        /// <param name="objects"></param>
        public static void Log(LogEventInfo logEventInfo, object[] objects)
        {
            OnLogHandler?.Invoke(logEventInfo, objects);
        }
    }
}