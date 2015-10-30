using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.Storage.Pickers;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

/*************************
*** A sample of content header
**************************

Ok
{
  Content-Length: 1016320
  Last-Modified: Sun, 04 Oct 2015 02:43:20 GMT
  Content-Type: application/vnd.ms-powerpoint
  Expires: Tue, 06 Oct 2015 09:22:21 GMT
  Content-Disposition: attachment; filename="%E8%A1%8C%E5%88%97%E5%BC%8F%EF%BC%88%E9%BB%91%EF%BC%891.ppt"; filename*=utf-8''%E8%A1%8C%E5%88%97%E5%BC%8F%EF%BC%88%E9%BB%91%EF%BC%891.ppt
}

**************************/

namespace BoxClient
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
#if DEBUG
            Input.Text = "BoxTest";
#endif
        }

        private async void Click_Me_Click(object sender, RoutedEventArgs e)
        {
            var szRequest = "Https://box.zjuqsc.com/item/get/";
            szRequest += Input.Text;
            Status.Text = "Sending request...\r\nPlease wait";
            HttpClient download = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await download.GetAsync(new Uri(szRequest));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetType());
                Status.Text = "Connection Error\r\nPlease check your network";
                return;
            }
            var headers = response.Headers;
            var content = response.Content;
            Debug.WriteLine(response.StatusCode);
            var reg = new Regex("filename=\"(.+)\"");
            var mat = reg.Match(content.Headers.ToString());
            if ((HttpStatusCode)404 == response.StatusCode)
            {
                Status.Text = "Error occurred...\r\nCheck your Code\r\nor try again";
                return;
            }
            Status.Text = "Response Got,Saving File";
            var filename = mat.Groups[0].Value;
            Debug.WriteLine(filename);

            var picker = new FileSavePicker
            {
                SuggestedFileName = filename,
                SuggestedStartLocation = PickerLocationId.Downloads
            };
            //picker.FileTypeChoices.Add("Any File Type", new List<string>() { "." });
            picker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>("All File", new List<string> { "*" }));
            var file = await picker.PickSaveFileAsync();
            if (null != file)
            {
                Debug.WriteLine(file.ToString());
                var buffer = await content.ReadAsBufferAsync();
                var write = await file.OpenTransactedWriteAsync();
                await write.Stream.WriteAsync(buffer);
                await write.CommitAsync();
                Status.Text = filename + " has been saved.";
            }
            else
                Status.Text = "Error:\r\nCannot Open the file";


#if DEBUG
            Debug.WriteLine(content.Headers);
            Status.Text = content.Headers.ToString();
#endif
            download.Dispose();
        }
    }
}
