using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Iot.Agent.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            BindingContext = ViewModel;

            Start().ConfigureAwait(false);
        }

        public async Task Start()
        {
            await ViewModel.ConnectAsync(App.WebAddress);
        }

        public MainViewModel ViewModel {get; private set; }
       
    }
}
