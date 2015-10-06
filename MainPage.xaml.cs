using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Diagnostics;
using System.Text.RegularExpressions;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

/*************************
*** A sample of content
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
            textBox.Text = "BoxTest";
#endif
        }

        private async void Click_Me_Click(object sender, RoutedEventArgs e)
        {
            string szRequest;
            szRequest = "Https://box.zjuqsc.com/item/get/";
            szRequest += textBox.Text;
            textBlock.Text = "Sending request...\r\nPlease wait";

#if DEBUG
            textBlock.Text = szRequest;
#endif

            HttpClient Download = new HttpClient();
            var Response = await Download.GetAsync(new Uri(szRequest));
            var headers = Response.Headers;
            var content = Response.Content;
            Debug.WriteLine(Response.StatusCode);
            Regex reg = new Regex("filename=\"(.+)\"");
            Match mat = reg.Match(content.Headers.ToString());
            if((HttpStatusCode)404==Response.StatusCode)
            {
                textBlock.Text = "Error occurred...\r\nCheck your Code\r\nor try again";
                return;
            }
            textBlock.Text = "Response Got,Saving File";
            string filename = null;
            filename = mat.Groups[0].Value;
            Debug.WriteLine(filename);
            try
            {
                FileStream fs = new FileStream(filename, FileMode.CreateNew);
            }
            catch(Exception ex)
            {

            }
#if DEBUG
            Debug.WriteLine(content.Headers);
            textBlock.Text = content.Headers.ToString();
#endif
            Download.Dispose();
        }
    }
}
