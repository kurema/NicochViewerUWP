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

using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using System.Threading.Tasks;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace NicochViewerUWP.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            NavigationViewMain.SelectedItem = NavigationViewMain.MenuItems[0];
            //LoadRemoteLibrary();
            //NavigationViewMain.BackRequested += NavigationViewMain_BackRequested;
            NavigationViewMain.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
        }

        public void LoadRemoteLibrary()
        {
            ShowLibrary(null);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(SettingPage), null);
                return;
            }
            if (args.SelectedItem is NavigationViewItem viewItem)
            {
                switch (viewItem?.Tag)
                {
                    case null: break;
                    case "Library": LoadRemoteLibrary(); return;
                }
            }
        }

        private void contentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationViewMain.IsBackEnabled = contentFrame.CanGoBack;
        }

        private async void ShowLibrary(string word)
        {
            var serverUrl = Storages.ConfigStorage.ServerUrl;
            var result = await Storages.RemoteCache.GetNicochInfoAsync();
            if (result == null)
            {
                await ShowAccessFailed();
                return;
            }
            contentFrame.Navigate(typeof(ChannelsPage), new ViewModels.ChannelsViewModel(result, serverUrl, word));
        }

        private async Task ShowAccessFailed()
        {
            var r = ResourceLoader.GetForCurrentView();
            await (new MessageDialog((r.GetString("Message_AccessFailed"))).ShowAsync());
            NavigationViewMain.SelectedItem = NavigationViewMain.SettingsItem;
            //contentFrame.Navigate(typeof(SettingPage), null);
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ShowLibrary(sender.Text);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //if (string.IsNullOrWhiteSpace(sender.Text)) ShowLibrary(null);
        }

        //private void NavigationViewMain_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        //{
        //    if (contentFrame.CanGoBack)
        //    {
        //        contentFrame.GoBack();
        //    }
        //}
    }
}
