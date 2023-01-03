using System;
using System.Windows;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SecretMessage.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        
        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, service) =>
                {
                    string firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY") 
                                            ?? throw new ArgumentNullException("FIREBASE_API_KEY");

                    service.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey)));
                    
                    service.AddSingleton<MainWindow>((services) => new MainWindow());
                })
                .Build();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}