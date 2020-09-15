using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace PodcastStats
{
    public class PodcastStatInfo
    {
        public List<PodcastStatInfoLine> PodcastStatInfoLines = new List<PodcastStatInfoLine>();

        public PodcastStatInfo(string url, string podcastName, DurationStyle durationStyle)
        {
            PodcastName = podcastName;
            Url = url;
            DurationStyle = durationStyle;
        }

        public string PodcastName { get; }

        public string Url { get; }

        public DurationStyle DurationStyle { get; }


        /// <summary>
        ///     The content in the 'duration' tag is frustratingly inconsistent. This handles the various formats I've seen and
        ///     spits out a consistent output in hh:mm:ss format.
        /// </summary>
        /// <param name="durationStyle">Duration style</param>
        /// <param name="durationStr">Input string</param>
        /// <returns>Formatted output in hh:mm:ss</returns>
        private string FormatDuration(DurationStyle durationStyle, string durationStr)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(durationStyle), durationStyle, null);
            }

            return durationStr;
        }

        public List<PodcastStatInfoLine> GetPodcastInfo()
        {
            var podcastStatInfoList = new List<PodcastStatInfoLine>();

            var reader = XmlReader.Create(Url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();
            Console.WriteLine(feed.Items.Count());


            foreach (var item in feed.Items)
            {
                var durationStr = "";

                var durationEls =
                    item.ElementExtensions.ReadElementExtensions<XmlElement>("duration",
                        "http://www.itunes.com/dtds/podcast-1.0.dtd");

                if (durationEls.Count <= 0)
                {
                    continue;
                }

                var durationEl = durationEls[0];


                if (durationEl == null || durationEl.InnerText == "0:00" || durationEl.InnerText == "" ||
                    durationEl.InnerText == "0")
                {
                    continue;
                }

                durationStr = durationEl.InnerText;

                durationStr = FormatDuration(DurationStyle, durationStr);

                if (durationStr == "")
                {
                    continue;
                }

                var psi = new PodcastStatInfoLine
                {
                    Duration = durationStr,
                    PublishDate = item.PublishDate.Date,
                    Title = item.Title.Text,
                    Id = item.Id,
                    Podcast = PodcastName
                };

                podcastStatInfoList.Add(psi);
            }

            PodcastStatInfoLines = podcastStatInfoList;
            return podcastStatInfoList;
        }
    }
}