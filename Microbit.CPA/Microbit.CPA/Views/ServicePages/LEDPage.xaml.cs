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
	public partial class LEDPage : ContentPage
	{
        private LedService _service;
        public LEDPage (LedService service)
		{
			InitializeComponent ();
            _service = service;
            BindingContext = _service;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Button b = new Button();
                    Tuple<int, int> coordinate = Tuple.Create(i, j);
                    b.Clicked += async (sender, e) =>
                    {
                        await _service.FlipLed(coordinate);
                    };
                    LedGrid.Children.Add(b, j, i);
                }
            }
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