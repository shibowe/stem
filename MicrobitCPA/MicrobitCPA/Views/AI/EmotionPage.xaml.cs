using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Diagnostics;

using MicrobitCPA.MicrobitUtils.Services;
using Plugin.BLE.Abstractions.Contracts;

namespace MicrobitCPA.Views.AI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmotionPage : ContentPage
    {
        EmotionServiceClient emotionClient;
        MediaFile photo;

        private LedService _service;

        public EmotionPage(LedService service)
        {
            InitializeComponent();

            _service = service;

            emotionClient = new EmotionServiceClient(Constants.EmotionApiKey, Constants.EmotionApiEndpoint);
        }

        async void OnTakePhotoButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                // Take photo
                if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
                {
                    photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = "emotion.jpg",
                        PhotoSize = PhotoSize.Small
                    });

                    if (photo != null)
                    {
                        image.Source = ImageSource.FromStream(photo.GetStream);
                    }
                }
                else
                {
                    await DisplayAlert("没有找到相机", "相机不可用.", "好的");
                }
            }
            catch (Exception exp)
            {
                await DisplayAlert("错误", exp.Message, "OK");
                throw;
            }
            ((Button)sender).IsEnabled = false;
            activityIndicator.IsRunning = true;

            // Recognize emotion
            try
            {
                if (photo != null)
                {
                    using (var photoStream = photo.GetStream())
                    {
                        Emotion[] emotionResult = await emotionClient.RecognizeAsync(photoStream);
                        if (emotionResult.Any())
                        {
                            // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                            emotionResultLabel.Text = emotionResult.FirstOrDefault().Scores.ToRankedList().FirstOrDefault().Key;

                            var recognizeValue = emotionResultLabel.Text.Trim();

                            //await _service.SendText(recognizeValue);
                            //await _service.Clear();

                            switch (recognizeValue)
                            {
                                case "Happiness":
                                    await _service.FlipLed(new Tuple<int, int>(0, 1));
                                    await _service.FlipLed(new Tuple<int, int>(0, 3));
                                    await _service.FlipLed(new Tuple<int, int>(1, 1));
                                    await _service.FlipLed(new Tuple<int, int>(1, 3));

                                    await _service.FlipLed(new Tuple<int, int>(3, 0));
                                    await _service.FlipLed(new Tuple<int, int>(3, 4));
                                    await _service.FlipLed(new Tuple<int, int>(4, 1));
                                    await _service.FlipLed(new Tuple<int, int>(4, 2));
                                    await _service.FlipLed(new Tuple<int, int>(4, 3));
                                    break;
                                case "Anger":
                                    await _service.FlipLed(new Tuple<int, int>(0, 1));
                                    await _service.FlipLed(new Tuple<int, int>(0, 3));
                                    await _service.FlipLed(new Tuple<int, int>(1, 1));
                                    await _service.FlipLed(new Tuple<int, int>(1, 3));

                                    await _service.FlipLed(new Tuple<int, int>(3, 1));
                                    await _service.FlipLed(new Tuple<int, int>(3, 2));
                                    await _service.FlipLed(new Tuple<int, int>(3, 3))
                                        ;
                                    await _service.FlipLed(new Tuple<int, int>(4, 0));
                                    await _service.FlipLed(new Tuple<int, int>(4, 4));
                                    break;
                                case "Disgust":
                                    break;
                                case "Surprise":
                                    await _service.FlipLed(new Tuple<int, int>(1, 0));
                                    await _service.FlipLed(new Tuple<int, int>(1, 1));
                                    await _service.FlipLed(new Tuple<int, int>(1, 3));
                                    await _service.FlipLed(new Tuple<int, int>(1, 4));
                                    await _service.FlipLed(new Tuple<int, int>(3, 1));
                                    await _service.FlipLed(new Tuple<int, int>(3, 2));
                                    await _service.FlipLed(new Tuple<int, int>(3, 3));
                                    await _service.FlipLed(new Tuple<int, int>(4, 1));
                                    await _service.FlipLed(new Tuple<int, int>(4, 2));
                                    await _service.FlipLed(new Tuple<int, int>(4, 3));
                                    break;
                                default:
                                    break;
                            }


                        }
                        photo.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            activityIndicator.IsRunning = false;
            ((Button)sender).IsEnabled = true;
        }
    }
}