﻿using System;
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
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();

            TextBoxServerUrl.Text = Storages.ConfigStorage.ServerUrl ?? "";
            CheckBoxPlayerType.IsChecked = Storages.ConfigStorage.PlayerTypeHtml;
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Storages.ConfigStorage.ServerUrl = (sender as TextBox)?.Text ?? Storages.ConfigStorage.ServerUrl;
            await Storages.RemoteCache.UpdateNicochInfoAsync();
        }

        private void CheckBoxPlayerType_Checked(object sender, RoutedEventArgs e)
        {
            Storages.ConfigStorage.PlayerTypeHtml = (sender as CheckBox)?.IsChecked ?? Storages.ConfigStorage.PlayerTypeHtml;
        }
    }
}
