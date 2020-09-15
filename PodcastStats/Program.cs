using System;

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
        }
    }
}