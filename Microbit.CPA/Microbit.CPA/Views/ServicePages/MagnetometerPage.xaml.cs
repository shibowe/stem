using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microbit.CPA.MicrobitUtils.Services;

namespace Microbit.CPA.Views.ServicePages
{
   

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MagnetometerPage : ContentPage
	{
        private MagnetometerService _service;
        public MagnetometerPage (MagnetometerService service)
		{
			InitializeComponent ();
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