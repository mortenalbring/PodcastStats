using System;

namespace PodcastStats
{

    /// <summary>
    /// Individual podcast episode information
    /// </summary>
    public class PodcastEpisodeInfo
    {
        public string Id { get; set; }
        public DateTime PublishDate { get; set; }

        public string Duration { get; set; }

        public string Title { get; set; }

        public string Podcast { get; set; }

        public string MakeStatLine()
        {
            return PublishDate.Date + "," + Duration + "," + Id + "," + Title;
        }

        public string MakeSimpleStatLine()
        {
            return PublishDate.Date + "," + Duration;
        }
    }
}