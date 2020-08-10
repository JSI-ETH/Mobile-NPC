using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileNPC.Services;
using MobileNPC.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Crashes;

namespace MobileNPC
{
    public partial class App : Application
    {

        // TODO: Move this to secure storage
        public static class AkeneoConfig
        {
            public static string AkeneoUrl = "https://ethiopia-demo.productcatalog.io";
            public static string Username = "admin";
            public static string Password = "Admin123";
            public static string ClientId = "1_c2vn1cpyyego80kkk8sw0w8wg8scss8s4so8c4o8s04gs0wwo";
            public static string ClientSecret = "1gcb5nxg4cn40c84g04wkg8gokgs4k48sow0soows8c84wk8c4";
        }
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = false;

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AkeneoDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("ios=b445c49d-aa72-4c2b-a2c5-c089b187be65;android=fd832c59-79e5-411d-ae7e-8374db42b146;", typeof(Distribute), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
