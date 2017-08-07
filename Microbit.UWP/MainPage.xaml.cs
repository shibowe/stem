using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Newtonsoft.Json;
using Windows.UI.Core;

namespace Microbit.UWP
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack
             ? AppViewBackButtonVisibility.Visible
             : AppViewBackButtonVisibility.Collapsed;

            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;



            base.OnNavigatedTo(e);
        }

        #region Emotion API
        private async void BtnInvokeAPI_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (null != file)
            {

                using (var inputStream = await file.OpenSequentialReadAsync())
                {
                    var readStream = inputStream.AsStreamForRead();
                    byte[] byteArray = new byte[readStream.Length];

                    await readStream.ReadAsync(byteArray, 0, byteArray.Length);

                    MakeRequest(file.Path.Trim(), byteArray);
                }
            }
            else
            {
                //this.tblResults.Text = "没有选择任何文件";
            }

        }

        private async void MakeRequest(string imagePath, byte[] byteData)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9c1e3b2860d8440c981ff1077c4cb6c2");

            string uri = "https://api.cognitive.azure.cn/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<List<Models.EmotionModel>>(responseContent);
                foreach (var item in result)
                {
                    //this.tblResults.Text = "惊喜值:" + item.Scores.surprise.ToString();
                }
            }
        }
        #endregion
    }
}
