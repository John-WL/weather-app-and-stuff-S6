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

namespace lab1_mais_2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private int count = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button buttonInc = FindViewById<Button>(Resource.Id.buttonSendUdp);
            TextView textCount = FindViewById<TextView>(Resource.Id.textView1);
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;

            UdpClient udpClient = new UdpClient();
            IPAddress ipAddress = new IPAddress(new Byte[] { 76, 71, 202, 19 });
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 10022);

            buttonInc.Click += delegate {

                Byte[] sendBytes = Encoding.ASCII.GetBytes("ALLO!");
                try
                {
                    udpClient.Send(sendBytes, sendBytes.Length, ipEndPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            };

            backgroundWorker.DoWork += new DoWorkEventHandler((sender, doWork) => {
                while (true)
                {
                    try
                    {
                        // Blocks until a message returns on this socket from a remote host.
                        Byte[] receiveBytes = udpClient.Receive(ref ipEndPoint);

                        string returnData = Encoding.ASCII.GetString(receiveBytes);

                        RunOnUiThread(() =>
                        {
                            textCount.Text = returnData.ToString();
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            });
            backgroundWorker.RunWorkerAsync();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}