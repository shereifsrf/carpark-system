using CPM.Shared.ClientServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            Services = ConfigureServices();
        }

        public new static App Current => (App)Application.Current;

        public static IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IClientParkService, ClientParkService>();
            // add http client
            services.AddScoped(sp => new HttpClient{
                BaseAddress = new Uri("http://tps-cpm.shereif.com/")
            });

            return services.BuildServiceProvider();
        }
    }
}
