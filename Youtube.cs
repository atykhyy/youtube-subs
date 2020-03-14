using System.Collections.Generic;
using System.Runtime.Serialization;

namespace youtube_subs
{
    [DataContract]
    public class YoutubeVideoSnippet
    {
        [DataMember (Name = "defaultAudioLanguage")]
        public string DefaultAudioLanguage { get ; set ; }
    }

    [DataContract]
    public class YoutubeVideoInfo
    {
        [DataMember (Name = "snippet")]
        public YoutubeVideoSnippet Snippet { get ; set ; }
    }

    [DataContract]
    public class YoutubeVideoData
    {
        [DataMember (Name = "items")]
        public YoutubeVideoInfo[] Infos { get ; set ; }
    }

    [DataContract]
    public class YoutubeCaption
    {
        [DataMember (Name = "url")]
        public string Url { get ; set ; }

        [DataMember (Name = "ext")]
        public string Ext { get ; set ; }
    }

    [DataContract]
    public class YoutubeVideoManifest
    {
        [DataMember (Name = "automatic_captions")]
        public Dictionary<string, YoutubeCaption[]> AutomaticCaptions { get ; set ; }

        [DataMember (Name = "title")]
        public string Title { get ; set ; }
    }
}
