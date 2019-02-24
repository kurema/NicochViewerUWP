using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace NicochViewerUWP.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class PlayerPageHtml : Page
    {
        public PlayerPageHtml()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ViewModels.VideoViewModel video && e.Parameter != null)
            {
                this.DataContext = new ViewModels.PlayerViewModel(video);
            }
            else if (e.Parameter is ViewModels.PlayerViewModel player && e.Parameter != null)
            {
                this.DataContext = player;
            }

            base.OnNavigatedTo(e);

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack
                ? Windows.UI.Core.AppViewBackButtonVisibility.Visible
                : Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += PlayerPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested -= PlayerPage_BackRequested;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;

            webView.NavigateToString("");
        }

        private void PlayerPage_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void Page_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var context = (DataContext as ViewModels.PlayerViewModel);
            if (context == null) return;

            var url = context.CurrentVideo?.PlayerUrl;
            if (!string.IsNullOrWhiteSpace(url)) webView.Source = new Uri(url);

            context.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ViewModels.PlayerViewModel.CurrentVideo))
                {
                    webView.Source = new Uri(context.CurrentVideo?.PlayerUrl);
                }
            };
        }

        private void WebView_ContainsFullScreenElementChanged(WebView sender, object args)
        {
            if (sender.ContainsFullScreenElement)
            {
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            }
            else
            {
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
        }
    }
}
