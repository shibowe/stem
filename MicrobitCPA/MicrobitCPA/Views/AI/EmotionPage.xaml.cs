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

namespace MicrobitCPA.Views.AI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmotionPage : ContentPage
    {
        EmotionServiceClient emotionClient;
        MediaFile photo;

        public EmotionPage()
        {
            InitializeComponent();

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
                    await DisplayAlert("No Camera", "Camera unavailable.", "OK");
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