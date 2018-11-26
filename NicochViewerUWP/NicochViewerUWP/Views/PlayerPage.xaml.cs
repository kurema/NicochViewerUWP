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
    public sealed partial class PlayerPage : Page
    {
        public PlayerPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ViewModels.VideoViewModel video && e.Parameter != null)
            {
                this.DataContext = new ViewModels.PlayerViewModel(video);
                //player.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(video.MovieUrl));
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

            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;

            player.Source = null;
            player.MediaPlayer.Pause();
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

            var url = context.CurrentVideo?.MovieUrl;
            if (!string.IsNullOrWhiteSpace(url)) player.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(url));

            context.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ViewModels.PlayerViewModel.CurrentVideo))
                {
                    player.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(context.CurrentVideo?.MovieUrl));
                }
            };

            if (controlNico != null)
            {
                controlNico.CommandNext = context.CommandNext;
                controlNico.CommandPrevious = context.CommandPrevious;
            }
        }


        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.Handled) return;
            var session = player.MediaPlayer.PlaybackSession;
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.C:
                    controlNico.IsCommentEnabled = !controlNico.IsCommentEnabled;
                    break;
                case Windows.System.VirtualKey.Right:
                    if (session.CanSeek)
                    {
                        session.Position += TimeSpan.FromSeconds(10);
                        //session.Position = TimeSpan.FromSeconds(10) + session.Position >= session.NaturalDuration ? session.NaturalDuration : session.Position + TimeSpan.FromSeconds(30);
                    }
                    break;
                case Windows.System.VirtualKey.Left:
                    if (session.CanSeek)
                    {
                        session.Position -= TimeSpan.FromSeconds(10);
                        //session.Position = TimeSpan.FromSeconds(10) < session.Position ? TimeSpan.Zero : session.Position - TimeSpan.FromSeconds(10);
                    }
                    break;
                case Windows.System.VirtualKey.Space:
                    switch (session.PlaybackState) {
                        case Windows.Media.Playback.MediaPlaybackState.Playing:
                            if (session.CanPause) player.MediaPlayer.Pause();
                            break;
                        default:
                            player.MediaPlayer.Play();
                            break;
                    }
                    break;
                case Windows.System.VirtualKey.Up:
                    player.MediaPlayer.Volume = Math.Min(1.0, player.MediaPlayer.Volume + 0.05);
                    break;
                case Windows.System.VirtualKey.Down:
                    player.MediaPlayer.Volume = Math.Max(0.0, player.MediaPlayer.Volume - 0.05);
                    break;
                case Windows.System.VirtualKey.M:
                    player.MediaPlayer.IsMuted = !player.MediaPlayer.IsMuted;
                    break;
            }
        }
    }
}
