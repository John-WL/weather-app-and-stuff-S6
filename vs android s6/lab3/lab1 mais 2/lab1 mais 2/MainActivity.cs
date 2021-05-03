using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.ComponentModel;
using Newtonsoft.Json;
using System.IO;

namespace lab1_mais_2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            TextView minTextView = FindViewById<TextView>(Resource.Id.textView2);
            TextView maxTextView = FindViewById<TextView>(Resource.Id.textView3);

            WebRequest webRequest = WebRequest.Create("http://dataservice.accuweather.com/forecasts/v1/daily/1day/50011?apikey=wMhkLdRDFjuUb5J9aUmxcIRsc6oExbtl&metric=true");
            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();

            String rawReception = new StreamReader(httpResponse
                .GetResponseStream())
                    .ReadToEnd();

            OneDayForecast forecast = JsonConvert.DeserializeObject<OneDayForecast>(rawReception);

            minTextView.Text = "Min: " + forecast.DailyForecasts[0].Temperature.Minimum.Value;
            maxTextView.Text = "Max: " + forecast.DailyForecasts[0].Temperature.Maximum.Value;

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}