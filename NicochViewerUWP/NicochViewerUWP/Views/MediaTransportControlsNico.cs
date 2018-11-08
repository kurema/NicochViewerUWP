using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace NicochViewerUWP.Views
{
    public class MediaTransportControlsNico : Windows.UI.Xaml.Controls.MediaTransportControls
    {
        private bool? IsCommentEnabledCache = null;
        public bool IsCommentEnabled { get => ButtonComment?.IsChecked ?? IsCommentEnabledCache ?? false; set
            {
                if (ButtonComment != null)
                {
                    ButtonComment.IsChecked = value;
                    IsCommentEnabledCache = null;
                }
                else
                {
                    IsCommentEnabledCache = value;
                }
            }
        }
        protected AppBarToggleButton ButtonComment=> GetTemplateChild("ShowCommentButton") as AppBarToggleButton;
        public event EventHandler<EventArgs> CommentEnabledChanged;

        protected AppBarButton ButtonNextTrackNico => GetTemplateChild("NextTrackNicoButton") as AppBarButton;
        public System.Windows.Input.ICommand _CommandNext;
        public System.Windows.Input.ICommand CommandNext { get => ButtonNextTrackNico?.Command; set
            {
                if (ButtonNextTrackNico != null)
                {
                    ButtonNextTrackNico.Command = value;
                    _CommandNext = null;
                }
                else
                {
                    _CommandNext = value;
                }
            }
        }

        protected AppBarButton ButtonPreviousTrackNico => GetTemplateChild("PreviousTrackNicoButton") as AppBarButton;
        public System.Windows.Input.ICommand _CommandPrevious;
        public System.Windows.Input.ICommand CommandPrevious
        {
            get => ButtonPreviousTrackNico?.Command; set
            {
                if (ButtonPreviousTrackNico != null)
                {
                    ButtonPreviousTrackNico.Command = value;
                    _CommandPrevious = null;
                }
                else
                {
                    _CommandPrevious = value;
                }
            }
        }

        public MediaTransportControlsNico()
        {
            this.DefaultStyleKey = typeof(MediaTransportControlsNico);
        }

        protected override void OnApplyTemplate()
        {
            // Find the custom button and create an event handler for its Click event.
            var button = this.ButtonComment;
            if (IsCommentEnabledCache != null) button.IsChecked = IsCommentEnabledCache ?? false;
            button.Click += CommentButton_Click;

            if (_CommandNext != null) CommandNext = _CommandNext;
            if (_CommandPrevious != null) CommandPrevious = _CommandPrevious;

            base.OnApplyTemplate();
        }

        private void CommentButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CommentEnabledChanged?.Invoke(this, new EventArgs());
        }
    }
}
