using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MobileNPC.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ZXingScannerPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }
        public void ZXingScannerPage_OnScanResult(ZXing.Result result)
        {
            IsScanning = !IsScanning;

            Device.BeginInvokeOnMainThread(async () =>
            {
                //await Navigation.PushModalAsync(new AboutPage());
                await DisplayAlert($"Scanned Result\n", $"Format:{result.BarcodeFormat}\nMetadata{result.ResultMetadata}\n{result.Text}", "OK");
            });
            IsScanning = !IsScanning;
        }

    }

}