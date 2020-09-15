using System;

namespace PodcastStats
{

    public class PodcastStatInfoLine
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