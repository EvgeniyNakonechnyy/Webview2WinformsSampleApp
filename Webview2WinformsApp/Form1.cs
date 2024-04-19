using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace Webview2WinformsApp
{
    public partial class Form1 : Form
    {
        private readonly string _url = "https://github.com/login";

        public Form1()
        {
            InitializeComponent();
            AttachControlEventHandlers();

            Resize += Form_Resize;

            InitializeAsync();
        }

        void AttachControlEventHandlers()
        {
            webView2.CoreWebView2InitializationCompleted += WebView2Control_CoreWebView2InitializationCompleted;
            webView2.NavigationStarting += WebView2Control_NavigationStarting;
            webView2.NavigationCompleted += WebView2Control_NavigationCompleted;
        }

        private async void InitializeAsync()
        {
            Log("EnsureCoreWebView2Async executed.");
            await webView2.EnsureCoreWebView2Async(null);

            webView2.CoreWebView2.CookieManager.DeleteAllCookies();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView2.Size = this.ClientSize - new System.Drawing.Size(webView2.Location);
        }

        private void WebView2Control_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                Log($"WebView2 creation failed with exception = {e.InitializationException}");
                Log("CoreWebView2InitializationCompleted failed");
                return;
            }

            webView2.CoreWebView2.Navigate(_url);

            Log("CoreWebView2InitializationCompleted succeeded");
        }

        private void WebView2Control_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            Log("NavigationStarting");
        }

        private void WebView2Control_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            Log("NavigationCompleted");
        }

        private void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now} : {message}");
        } 

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Resize -= Form_Resize;
            if (webView2 != null)
            {
                webView2.Stop();
                webView2.Dispose();
            }
        }
    }
}
