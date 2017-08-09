using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MicrobitCPA.MicrobitUtils.Services;

namespace MicrobitCPA.Views.ServicePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TemperaturePage : ContentPage
    {
        private TemperatureService _service;
        public TemperaturePage(TemperatureService service)
        {
            InitializeComponent();

            _service = service;
            BindingContext = _service;
        }
        protected override void OnAppearing()
        {
            _service.LoadCharacteristics();
        }

        protected override void OnDisappearing()
        {
            _service.StopUpdates();
        }
    }
}