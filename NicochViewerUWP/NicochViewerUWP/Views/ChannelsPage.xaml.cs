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
    public sealed partial class ChannelsPage : Page
    {
        public ChannelsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is ViewModels.ChannelsViewModel channels)
            {
                this.DataContext = channels;
                //this.ChannelsSource.Source = channels.Channels;
                if (!string.IsNullOrWhiteSpace(Storages.History.LastPlayedId))
                {
                    var target = channels?.Channels?.SelectMany(a => a.Videos).FirstOrDefault(a => a.Id == Storages.History.LastPlayedId);
                    if (target != null) SemanticZoom1.ZoomedInView.MakeVisible(new SemanticZoomLocation() { Item = target });
                }
            }

            base.OnNavigatedTo(e);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ViewModels.VideoViewModel video)
            {
                if (!string.IsNullOrWhiteSpace(video.MovieUrl))
                {
                    if (Window.Current.Content is Frame f)
                    {
                        Storages.History.LastPlayedId = video.Id;
                        ViewModels.ChannelViewModel channel = (this.DataContext as ViewModels.ChannelsViewModel)?.Channels?.FirstOrDefault(a => a.Videos.Contains(video));
                        f.Navigate(typeof(PlayerPage), new ViewModels.PlayerViewModel(video,channel));
                    }
                }
            }
        }
    }
}
