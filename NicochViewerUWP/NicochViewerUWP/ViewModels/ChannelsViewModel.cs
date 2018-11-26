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
        public ObservableCollection<ChannelViewModel> Channels { get
            {
                return new ObservableCollection<ChannelViewModel>(_Channels.Select(a => a.GetQuery(QueryWords)).Where(a => a.Videos.Count > 0));
            }
            set => SetProperty(ref _Channels, value); }

        public string[] _QueryWords=new string[0];
        public string[] QueryWords
        {
            get => _QueryWords ?? new string[0];
            set
            {
                SetProperty(ref _QueryWords, value);
                OnPropertyChanged(nameof(Channels));
            }
        }

        public ChannelsViewModel()
        {
        }

        public ChannelsViewModel(Json.NicochInfo info, string sourcePath,string queryWord=null)
        {
            this._Channels = new ObservableCollection<ChannelViewModel>(info?.recorded_channels?.Where(a => a?.videos?.Length > 0)?.Select(a => new ChannelViewModel(a, sourcePath)) ?? new ChannelViewModel[0]);
            this._QueryWords = string.IsNullOrWhiteSpace(queryWord) ? new string[0] : queryWord.Split(' ','　').Where(a=>!string.IsNullOrWhiteSpace(a)).ToArray();
        }
    }

    public class ChannelViewModel : BaseViewModel
    {
        private string _Title;
        public string Title { get => _Title; set => SetProperty(ref _Title, value); }

        private ObservableCollection<VideoViewModel> _Videos;
        public ObservableCollection<VideoViewModel> Videos
        {
            get => new ObservableCollection<VideoViewModel>(SearchVideo(this._Title, this._Videos, this._QueryWords));
            set => SetProperty(ref _Videos, value);
        }

        public VideoViewModel FirstVideo => Videos.Count > 0 ? Videos[0] : new VideoViewModel();

        public static IEnumerable<VideoViewModel> SearchVideo(string title,IEnumerable<VideoViewModel> videos,string[] queries)
        {
            if (queries == null || queries.Length == 0) return videos;
            foreach(var item in queries)
            {
                if (title.Contains(item)) return videos;
            }
            return videos.Where(a => queries.Any(b => a.Title.Contains(b)));
        }

        public string[] _QueryWords = new string[0];
        public string[] QueryWords
        {
            get => _QueryWords ?? new string[0];
            set
            {
                SetProperty(ref _QueryWords, value);
                OnPropertyChanged(nameof(Videos));
            }
        }

        public ChannelViewModel GetQuery(string[] queries)
        {
            return new ChannelViewModel(this._Title, this._Videos, queries);
        }

        public ChannelViewModel()
        {
        }

        public ChannelViewModel(Json.ChannelRecorded channel, string sourcePath)
        {
            this._Title = channel.channnel_id;
            this._Videos = new ObservableCollection<VideoViewModel>(channel?.videos?.Select((a) => new VideoViewModel(a, sourcePath)) ?? new VideoViewModel[0]);
        }

        public ChannelViewModel(string Title, ObservableCollection<VideoViewModel> Videos, string[] QueryWords)
        {
            _Title = Title ?? throw new ArgumentNullException(nameof(Title));
            _Videos = Videos ?? throw new ArgumentNullException(nameof(Videos));
            _QueryWords = QueryWords ?? throw new ArgumentNullException(nameof(QueryWords));
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
