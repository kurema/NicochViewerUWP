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

        public async void LoadRemoteLibrary()
        {
            var serverUrl = Storages.ConfigStorage.ServerUrl;
            var result = await Json.Loader.GetNicochInfo(serverUrl);
            if (result == null)
            {
                var r = ResourceLoader.GetForCurrentView();
                NavigationViewMain.SelectedItem = NavigationViewMain.SettingsItem;
                //contentFrame.Navigate(typeof(SettingPage), null);
                await (new MessageDialog((r.GetString("Message_AccessFailed"))).ShowAsync());
                return;
            }
            contentFrame.Navigate(typeof(ChannelsPage), new ViewModels.ChannelsViewModel(result, serverUrl));
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

        //private void NavigationViewMain_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        //{
        //    if (contentFrame.CanGoBack)
        //    {
        //        contentFrame.GoBack();
        //    }
        //}
    }
}
