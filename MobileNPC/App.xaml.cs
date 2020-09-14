using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileNPC.Services;
using MobileNPC.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;
using System.Linq;
using MobileNPC.Configuration;

namespace MobileNPC
{
    public partial class App : Application
    {

        public static class EnvironmentVariables
        {
            public const string AkeneoUrl = "AKENEO_URL";
            public const string Username = "AKENEO_USERNAME";
            public const string Password = "AKENEO_PASSWORD";
            public const string ClientId = "AKENEO_CLIENT_ID";
            public const string ClientSecret = "AKENEO_CLIENT_SECRET";
            public const string Categories = "AKENEO_CATEGORIES";
            public const string Family = "AKENEO_FAMILY";
            public const string AkeneoConfigUrl = "AKENEO_CONFIG_URL";
        }

        // TODO: Move this to secure storage
        public static class AkeneoConfig
        {
            public static string AkeneoUrl = Environment.GetEnvironmentVariable(EnvironmentVariables.AkeneoUrl);
            public static string Username = Environment.GetEnvironmentVariable(EnvironmentVariables.Username);
            public static string Password = Environment.GetEnvironmentVariable(EnvironmentVariables.Password);
            public static string ClientId = Environment.GetEnvironmentVariable(EnvironmentVariables.ClientId);
            public static string ClientSecret = Environment.GetEnvironmentVariable(EnvironmentVariables.ClientSecret);
            public static IEnumerable<string> Categories = Environment.GetEnvironmentVariable(EnvironmentVariables.Categories)?.Split(',').ToList() ?? new List<string>();
            public static string AkeneoFamily = Environment.GetEnvironmentVariable(EnvironmentVariables.Family);
            public static string AkeneoConfigUrl = Environment.GetEnvironmentVariable(EnvironmentVariables.AkeneoConfigUrl);
            public static AppConfiguration Configuration;
        }
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = false;

        public App()
        {
            AkeneoConfig.Configuration = AppConfiguration.Create(AkeneoConfig.AkeneoConfigUrl);
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AkeneoDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=2978adb1-3a4b-4c3f-89fc-22cdd61d0c9c;" +
                  "ios=4675c0bf-7208-4873-969a-235251ceb5b0;",
                  typeof(Analytics), typeof(Crashes), typeof(Distribute));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
