using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MicrobitCPA.MicrobitUtils.Services;
using MicrobitCPA.Views.AI;

using Microsoft.ProjectOxford.Emotion;
using Plugin.Media.Abstractions;


namespace MicrobitCPA.Views.ServicePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LEDPage : ContentPage
    {
        private LedService _service;
        public LEDPage(LedService service)
        {
            InitializeComponent();

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

        private void BtnInvokeAzure_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new EmotionPage(_service));
        }
    }
}