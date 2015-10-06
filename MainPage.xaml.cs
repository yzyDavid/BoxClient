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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

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
            if((HttpStatusCode)404==Response.StatusCode)
            {
                textBlock.Text = "Error occurred...\r\nCheck your Code\r\nor try again";
                return;
            }
            textBlock.Text = "Response Got,Saving File";
            string filename = null;
            //FileStream fs = new FileStream(filename, FileMode.CreateNew);
#if DEBUG
            Debug.WriteLine(content.Headers);
            textBlock.Text = content.Headers.ToString();
#endif
            Download.Dispose();
        }
    }
}
