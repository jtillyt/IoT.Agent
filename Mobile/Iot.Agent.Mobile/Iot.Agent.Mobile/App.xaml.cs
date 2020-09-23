using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Iot.Agent.Mobile
{
    public partial class App : Application
    {
        //public static string WebAddress => "http://127.0.0.1:5001/nodehub";
        public static string WebAddress => "http://192.168.1.200:5001/nodehub";

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
