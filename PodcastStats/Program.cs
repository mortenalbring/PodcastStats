using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace PodcastStats
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var nnpi = new PodcastStatInfo("https://feeds.99percentinvisible.org/", "99% Invisible", DurationStyle.mmss);

            nnpi.GetPodcastInfo();
            
            var giantbombcast = new PodcastStatInfo("https://www.giantbomb.com/feeds/podcast/", "Giant Bombcast",
                DurationStyle.secondsDuration);

            var beastcast = new PodcastStatInfo("https://www.giantbomb.com/podcast-xml/beastcast/", "Giant Beastcast",
                DurationStyle.secondsDuration);

            var altfone = new PodcastStatInfo("https://www.giantbomb.com/podcast-xml/altf1/", "ALT F1",
                DurationStyle.secondsDuration);

            var asg = new PodcastStatInfo("https://www.giantbomb.com/podcast-xml/all-systems-goku", "All Systems Goku",
                DurationStyle.secondsDuration);

            var bombcastaftermath = new PodcastStatInfo("https://www.giantbomb.com/podcast-xml/bombcast-aftermath/",
                "Bombcast Aftermath",
                DurationStyle.secondsDuration);

            
            var replyall = new PodcastStatInfo("https://feeds.megaphone.fm/replyall", "Reply All",
                DurationStyle.secondsDuration);

            var joerogan = new PodcastStatInfo("http://joeroganexp.joerogan.libsynpro.com/rss", "Joe Rogan Experience",
                DurationStyle.hhmmssMixed);

            var combinedGraph = new CombinedPodcastGraph();
            combinedGraph.Add(giantbombcast);
            combinedGraph.Add(beastcast);
            combinedGraph.Add(altfone);
            combinedGraph.Add(asg);
            combinedGraph.Add(bombcastaftermath);


            var data = combinedGraph.MakePlot();

            foreach (var d in data)
            {
                Console.WriteLine(d);
            }

            //
            // var output = GetPodcastInfo("https://feeds.99percentinvisible.org/", "99% Invisible", DurationStyle.mmss);
            // var output2 = GetPodcastInfo("https://www.giantbomb.com/feeds/podcast/", "Giant Bombcast",
            //     DurationStyle.secondsDuration);
            // var output3 = GetPodcastInfo("https://www.giantbomb.com/podcast-xml/beastcast/", "Giant Beastcast",
            //     DurationStyle.secondsDuration);
            //
            // var output4 = GetPodcastInfo("https://feeds.megaphone.fm/replyall", "Reply All",
            //     DurationStyle.secondsDuration);
            //
            // var output5 = GetPodcastInfo("http://joeroganexp.joerogan.libsynpro.com/rss", "Joe Rogan Experience",
            //     DurationStyle.hhmmssMixed);
            //
            // var output6 = GetPodcastInfo("https://www.giantbomb.com/podcast-xml/altf1/", "ALT F1",
            //     DurationStyle.secondsDuration);
            // var output7 = GetPodcastInfo("https://www.giantbomb.com/podcast-xml/all-systems-goku", "All Systems Goku",
            //     DurationStyle.secondsDuration);
            //
            // var output8 = GetPodcastInfo("https://www.giantbomb.com/podcast-xml/bombcast-aftermath/",
            //     "Bombcast Aftermath",
            //     DurationStyle.secondsDuration);
            //
            //
            // var gb = new List<PodcastStatInfoLine>();
            // gb.AddRange(output2);
            // gb.AddRange(output3);
            // gb.AddRange(output6);
            // gb.AddRange(output7);
            // gb.AddRange(output8);
            //
            //
            // var outputs = new List<PodcastStatInfoLine>();
            // outputs.AddRange(output);
            // outputs.AddRange(output2);
            // outputs.AddRange(output3);
            // outputs.AddRange(output4);
            // outputs.AddRange(output5);
            //
            // var dataText = AggregateDataForPlot(gb);
            //
            // foreach (var dt in dataText) Console.WriteLine(dt);
        }
    }
}