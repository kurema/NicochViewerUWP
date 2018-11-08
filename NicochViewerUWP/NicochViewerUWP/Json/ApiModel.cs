using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace NicochViewerUWP.Json
{
    [DataContract]
    public class NicochInfo
    {
        [DataMember]
        public ChannelRecorded[] recorded_channels { get; set; }
        [DataMember]
        public ChannnelRecording[] recording_channels { get; set; }
    }

    [DataContract]
    public class ChannnelRecording
    {
        [DataMember]
        public string channel_url { get; set; }
        [DataMember]
        public string channel_id { get; set; }
    }

    [DataContract]
    public class ChannelRecorded
    {
        [DataMember]
        public string channnel_id { get; set; }
        [DataMember]
        public Video[] videos { get; set; }
    }

    [DataContract]
    public class Video
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string movie_url { get; set; }
        [DataMember]
        public string thumbnail_url { get; set; }
        [DataMember]
        public string comment_url { get; set; }
    }
}
