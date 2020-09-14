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
            var output0 = GetPodcastInfo("http://joeroganexp.joerogan.libsynpro.com/rss", "Joe Rogan Experience",
                DurationStyle.hhmmssMixed);

            var output = GetPodcastInfo("https://feeds.99percentinvisible.org/", "99% Invisible", DurationStyle.mmss);
            var output2 = GetPodcastInfo("https://www.giantbomb.com/feeds/podcast/", "Giant Bombcast",
                DurationStyle.secondsDuration);
            var output3 = GetPodcastInfo("https://www.giantbomb.com/podcast-xml/beastcast/", "Giant Beastcast",
                DurationStyle.secondsDuration);


            var outputs = new List<PodcastStatInfo>();
            outputs.AddRange(output);
            outputs.AddRange(output2);
            outputs.AddRange(output3);
            outputs.AddRange(output0);

            var allDates = outputs.Select(e => e.PublishDate).Distinct().ToList();
            var podcastTypes = outputs.Select(e => e.Podcast).Distinct().ToList();

            var header = "Date,";

            foreach (var p in podcastTypes) header = header + p + ",";

            var dataText = new List<string>();
            dataText.Add(header);
            foreach (var d in allDates)
            {
                var outputStr = d.ToString("yyyy/MM/dd");
                foreach (var p in podcastTypes)
                {
                    var dateDur = "";

                    var match = outputs.Where(e => e.PublishDate == d && e.Podcast == p).ToList();
                    if (match.Count == 0)
                    {
                        dateDur = "";
                    }

                    if (match.Count == 1)
                    {
                        dateDur = match.First().Duration;
                    }

                    if (match.Count > 1)
                    {
                        //todo handle case where multiple podcasts added on same date
                        dateDur = match.First().Duration;
                    }

                    outputStr = outputStr + "," + dateDur;
                }

                dataText.Add(outputStr);
            }

            foreach (var dt in dataText) Console.WriteLine(dt);
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


                var durationEls =
                    item.ElementExtensions.ReadElementExtensions<XmlElement>("duration",
                        "http://www.itunes.com/dtds/podcast-1.0.dtd");

                if (durationEls.Count > 0)
                {
                    var durationEl =
                        durationEls[0];


                    if (durationEl != null && durationEl.InnerText != "0:00" && durationEl.InnerText != "" &&
                        durationEl.InnerText != "0")
                    {
                        durationStr = durationEl.InnerText;

                        durationStr = FormatDuration(durationStyle, durationStr);

                        if (durationStr != "")
                        {
                            var psi = new PodcastStatInfo();
                            psi.Duration = durationStr;
                            psi.PublishDate = item.PublishDate.Date;
                            psi.Title = item.Title.Text;
                            psi.Id = item.Id;
                            psi.Podcast = podcastName;
                            output2.Add(psi);
                        }
                    }
                }
            }

            return output2;
        }

        private static string FormatDuration(DurationStyle durationStyle, string durationStr)
        {
            switch (durationStyle)
            {
                case DurationStyle.hhmmss:
                    break;
                case DurationStyle.hhmmssMixed:
                    var splits = durationStr.Split(':');

                    var hhInt1 = 0;
                    var mmInt1 = 0;
                    var ssInt1 = 0;
                    if (splits.Length == 2)
                    {
                        var mm = splits[0];
                        var ss = splits[1];

                        var mmIntSuccess = int.TryParse(mm, out mmInt1);
                        var ssIntSuccess = int.TryParse(ss, out ssInt1);
                        if (!mmIntSuccess || !ssIntSuccess)
                        {
                            return "";
                        }
                    }
                    else if (splits.Length == 3)
                    {
                        var hh = splits[0];
                        var mm = splits[1];
                        var ss = splits[2];


                        var hhIntSuccess = int.TryParse(hh, out hhInt1);
                        var mmIntSuccess = int.TryParse(mm, out mmInt1);
                        var ssIntSuccess = int.TryParse(ss, out ssInt1);
                        if (!hhIntSuccess || !mmIntSuccess || !ssIntSuccess)
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "";
                    }


                    var timetotal = hhInt1 * 60 * 60 + mmInt1 * 60 + ssInt1;

                    var timess = TimeSpan.FromSeconds(timetotal);

                    var strr = timess.ToString(@"hh\:mm\:ss");

                    return strr;

                    break;
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

                    var mmIntSuccess = int.TryParse(mm, out var mmInt);
                    var ssIntSuccess = int.TryParse(ss, out var ssInt);
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
            hhmmss,
            hhmmssMixed,
            mmss,
            secondsDuration
        }

        private class PodcastStatInfo
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
}