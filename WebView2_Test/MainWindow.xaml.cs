using System;
using Microsoft.Web.WebView2.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace WebView2_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            InitializeAsync();
        }


        async void InitializeAsync()
        {
            // WebView2との通信系初期化
            await webView.EnsureCoreWebView2Async(null);// InitializeAsyncの初期化は非同期

            webView.CoreWebView2.NavigationCompleted += NavigationCompleted;//コンテンツ読み込み時にフォルダ読みこみを実行
            // 初期フォルダをマイピクチャに設定
            addressBar.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // 同封のwebシステムを読み込む
            webView.CoreWebView2.Navigate(System.AppDomain.CurrentDomain.BaseDirectory + "Resources\\WebApp\\index.html");

            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");

        }


        private void NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            GetFileList(addressBar.Text);
        }

        void GetFileList(String path)
        {

            try
            {

                path = path.Replace(@"file:///", "");
                Debug.WriteLine("ファイル名" + path);
                

                string[] names = Directory.EnumerateFiles(path)//ファイル取得
                    .Where(s =>//拡張子で絞り込み(大文字・小文字区別なし)
                        s.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase)
                        ||
                        s.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)
                        ||
                        s.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase)
                    ).ToArray();

                //ファイル情報をwebviewに送信
                webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(names));
            }
            catch (Exception ed)
            {
                Console.WriteLine(ed.ToString());
            }
        }


        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                GetFileList(addressBar.Text);
            }


        }

    }
}
