using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MobileNPC.Services;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MobileNPC.Views
{
	public partial class HomePage : ContentPage
	{
		ZXingScannerPage scanPage;
		Button buttonScanDefaultOverlay;
		Button buttonScanCustomOverlay;
		Button buttonScanContinuously;
		Button buttonScanContinuousCustomPage;
		Button buttonScanCustomPage;
		Button buttonGenerateBarcode;

		private readonly IDataStore<Models.Item> _dataStore = DependencyService.Get<IDataStore<Models.Item>>();

		public HomePage() : base()
		{
			buttonScanDefaultOverlay = new Button
			{
				Text = "Scan with Default Overlay",
				AutomationId = "scanWithDefaultOverlay",
			};
			buttonScanDefaultOverlay.Clicked += async delegate
			{
				scanPage = new ZXingScannerPage();
				scanPage.OnScanResult += (result) =>
				{
					scanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(async () =>
					{
						await Navigation.PopAsync();
						await DisplayAlert("Scanned Barcode", result.Text, "OK");
					});
				};

				await Navigation.PushAsync(scanPage);
			};


			buttonScanCustomOverlay = new Button
			{
				Text = "Scan with Custom Overlay",
				AutomationId = "scanWithCustomOverlay",
			};
			buttonScanCustomOverlay.Clicked += async delegate
			{
				// Create our custom overlay
				var customOverlay = new StackLayout
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand
				};
				var torch = new Button
				{
					Text = "Toggle Torch"
				};
				torch.Clicked += delegate
				{
					scanPage.ToggleTorch();
				};
				customOverlay.Children.Add(torch);

				scanPage = new ZXingScannerPage(new ZXing.Mobile.MobileBarcodeScanningOptions { AutoRotate = true }, customOverlay: customOverlay);
				scanPage.OnScanResult += (result) =>
				{
					scanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(async () =>
					{
						await Navigation.PopAsync();
						await DisplayAlert("Scanned Barcode", result.Text, "OK");
					});
				};
				await Navigation.PushAsync(scanPage);
			};


			buttonScanContinuously = new Button
			{
				Text = "Start Scan",
				AutomationId = "scanContinuously",
			};
			buttonScanContinuously.Clicked += async delegate
			{
				scanPage = new ZXingScannerPage(new ZXing.Mobile.MobileBarcodeScanningOptions
				{
					DelayBetweenContinuousScans = 3000,
					TryHarder = true,
					AssumeGS1 = true,
					AutoRotate = true,
				});
				scanPage.OnScanResult += (result) =>
					Device.BeginInvokeOnMainThread(async () =>
					{
						//await DisplayAlert("Scanned Barcode", result.Text, "OK");
						await Navigation.PopAsync();
						string gtin = string.Empty;
						var barcode = result.Text;
						var gtinRegEx = new System.Text.RegularExpressions.Regex("^(\\d{8}|\\d{12,14})$");
						if (gtinRegEx.IsMatch(barcode))
							gtin = barcode;
						else
						{
							var identifiers = GS1.Parse(barcode);
							if (identifiers.Any(i => i.Key.AI == "01"))
							{
								var gtinIdentifier = identifiers.Single(i => i.Key.AI == "01");
								if (gtinRegEx.IsMatch(gtinIdentifier.Value))
									gtin = gtinIdentifier.Value;
							}
						}
						if (string.IsNullOrEmpty(gtin))
							await DisplayAlert("Error!", $"Could not parse GTIN from the supplied barcode! \n{barcode}", "OK");
						else
                        {
							var selectedItem = await _dataStore.GetItemAsync(gtin);
							if (selectedItem != null)
								await Navigation.PushAsync(new ItemDetailPage(new ViewModels.ItemDetailViewModel(selectedItem)));
							else
								await DisplayAlert("Sorry!", $"The item with the specified GTIN `{gtin}` could not be found. Please try again!", "OK");
						}
						
					});

				await Navigation.PushAsync(scanPage);
			};

			buttonScanCustomPage = new Button
			{
				Text = "Start Scan",
				AutomationId = "scanWithCustomPage",
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			};
			buttonScanCustomPage.Clicked += async delegate
			{
				var customScanPage = new CustomScanPage();
				await Navigation.PushAsync(customScanPage);
			};


			var stack = new StackLayout();
			stack.VerticalOptions = LayoutOptions.Center;
			stack.Children.Add(buttonScanContinuously);

			Content = stack;
		}
	}
}