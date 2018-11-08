using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Collections.ObjectModel;

namespace NicochViewerUWP.ViewModels
{
    public class ChannelsViewModel : BaseViewModel
    {
        private ObservableCollection<ChannelViewModel> _Channels;
        public ObservableCollection<ChannelViewModel> Channels { get => _Channels; set => SetProperty(ref _Channels, value); }

        public ChannelsViewModel()
        {
        }

        public ChannelsViewModel(Json.NicochInfo info, string sourcePath)
        {
            this._Channels = new ObservableCollection<ChannelViewModel>(info?.recorded_channels?.Where(a => a?.videos?.Length > 0)?.Select(a => new ChannelViewModel(a, sourcePath)) ?? new ChannelViewModel[0]);
        }
    }

    public class ChannelViewModel : BaseViewModel
    {
        private string _Title;
        public string Title { get => _Title; set => SetProperty(ref _Title, value); }

        private ObservableCollection<VideoViewModel> _Videos;
        public ObservableCollection<VideoViewModel> Videos { get => _Videos; set => SetProperty(ref _Videos, value); }

        public VideoViewModel FirstVideo => Videos.Count > 0 ? Videos[0] : new VideoViewModel();

        public ChannelViewModel()
        {
        }

        public ChannelViewModel(Json.ChannelRecorded channel, string sourcePath)
        {
            this._Title = channel.channnel_id;
            this._Videos = new ObservableCollection<VideoViewModel>(channel?.videos?.Select((a) => new VideoViewModel(a, sourcePath)) ?? new VideoViewModel[0]);
        }
    }

    public class VideoViewModel : BaseViewModel
    {

        private string _Id;
        public string Id { get => _Id; set => SetProperty(ref _Id, value); }

        private string _Title;
        public string Title { get => _Title; set => SetProperty(ref _Title, value); }

        private string _ThumbnailUrl;
        public string ThumbnailUrl { get => _ThumbnailUrl; set => SetProperty(ref _ThumbnailUrl, value); }

        private string _MovieUrl;
        public string MovieUrl { get => _MovieUrl; set => SetProperty(ref _MovieUrl, value); }

        public VideoViewModel()
        {
        }

        public VideoViewModel(Json.Video video,string sourcePath)
        {
            this._Title = video.title;
            this._ThumbnailUrl = System.IO.Path.Combine(sourcePath, video.thumbnail_url);
            this._MovieUrl = System.IO.Path.Combine(sourcePath, video.movie_url);
            this._Id = video.id;
        }

    }
}
