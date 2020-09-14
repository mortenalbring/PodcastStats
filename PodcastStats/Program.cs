using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace PodcastStats
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var output = GetPodcastInfo("https://feeds.99percentinvisible.org/", "99% Invisible", DurationStyle.mmss);
            //   var output2 = GetPodcastInfo("https://www.giantbomb.com/feeds/podcast/", "Giant Bombcast", DurationStyle.secondsDuration);

            // foreach (var d in output)
            // {
            //     Console.WriteLine(d.Key.Date + ","  + d.Value);
            // }

            foreach (var d in output) Console.WriteLine(d.MakeSimpleStatLine());
        }

        private static List<PodcastStatInfo> GetPodcastInfo(string url, string podcastName, DurationStyle durationStyle)
        {
            var output2 = new List<PodcastStatInfo>();

            var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();
            Console.WriteLine(feed.Items.Count());


            foreach (var item in feed.Items)
            {
                var durationStr = "";

                var durationEl =
                    item.ElementExtensions.ReadElementExtensions<XmlElement>("duration",
                        "http://www.itunes.com/dtds/podcast-1.0.dtd")[0];

                if (durationEl != null && durationEl.InnerText != "0:00" && durationEl.InnerText != "" &&
                    durationEl.InnerText != "0")
                {
                    durationStr = durationEl.InnerText;

                    durationStr = FormatDuration(durationStyle, durationStr);

                    if (durationStr != "")
                    {
                        var psi = new PodcastStatInfo();
                        psi.Duration = durationStr;
                        psi.PublishDate = item.PublishDate;
                        psi.Title = item.Title.Text;
                        psi.Id = item.Id;
                        psi.Podcast = podcastName;
                        output2.Add(psi);
                    }
                }
            }

            return output2;
        }

        private static string FormatDuration(DurationStyle durationStyle, string durationStr)
        {
            switch (durationStyle)
            {
                case DurationStyle.secondsDuration:
                {
                    int durInt;
                    var durSuccess = int.TryParse(durationStr, out durInt);
                    if (durSuccess)
                    {
                        var times = TimeSpan.FromSeconds(durInt);

                        var str = times.ToString(@"hh\:mm\:ss");
                        durationStr = str;
                    }
                    else
                    {
                        durationStr = "";
                    }

                    break;
                }
                case DurationStyle.mmss:
                {
                    var split = durationStr.Split(':');
                    if (split.Length != 2)
                    {
                        return "";
                    }

                    var mm = split[0];
                    var ss = split[1];

                    int mmInt;
                    var mmIntSuccess = int.TryParse(mm, out mmInt);
                    int ssInt;
                    var ssIntSuccess = int.TryParse(ss, out ssInt);

                    if (!mmIntSuccess || !ssIntSuccess)
                    {
                        return "";
                    }

                    var time = mmInt * 60 + ssInt;

                    var times = TimeSpan.FromSeconds(time);

                    var str = times.ToString(@"hh\:mm\:ss");

                    return str;
                }
            }

            return durationStr;
        }

        private enum DurationStyle
        {
            mmss,
            secondsDuration
        }

        private class PodcastStatInfo
        {
            public string Id { get; set; }
            public DateTimeOffset PublishDate { get; set; }

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
}