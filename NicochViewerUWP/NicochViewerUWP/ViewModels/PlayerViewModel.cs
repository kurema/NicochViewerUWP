using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace NicochViewerUWP.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        private ChannelViewModel _Channel;
        public ChannelViewModel Channel { get => _Channel; set => SetProperty(ref _Channel, value); }

        private VideoViewModel _CurrentVideo;
        public VideoViewModel CurrentVideo { get => _CurrentVideo; set { SetProperty(ref _CurrentVideo, value); CommandPrevious.OnCanExecuteChanged(); CommandNext.OnCanExecuteChanged(); } }

        private DelegateCommand _CommandPrevious;
        public DelegateCommand CommandPrevious => _CommandPrevious = _CommandPrevious ?? new DelegateCommand((_) => CurrentVideo = ShiftVideo(-1).Item2, (_) => ShiftVideo(-1).Item1);

        private DelegateCommand _CommandNext;
        public DelegateCommand CommandNext => _CommandNext = _CommandNext ?? new DelegateCommand((_) => CurrentVideo = ShiftVideo(+1).Item2, (_) => ShiftVideo(+1).Item1);

        public PlayerViewModel(VideoViewModel CurrentVideo, ChannelViewModel Channel=null)
        {
            _CurrentVideo = CurrentVideo ?? throw new ArgumentNullException(nameof(CurrentVideo));
            _Channel = Channel;
        }

        public PlayerViewModel()
        {
            _CurrentVideo = new VideoViewModel();
        }

        protected (bool,VideoViewModel) ShiftVideo(int shiftValue)
        {
            var videos = Channel?.Videos?.OrderBy(a => a.Id).ToArray();
            if (videos == null || videos.Count() <= 0) return (false, null);
            var index = Array.IndexOf(videos, CurrentVideo) + shiftValue;
            if(index<0 || index >= videos.Count())
            {
                return (false, null);
            }
            return (true, videos[index]);
        }
    }
}
