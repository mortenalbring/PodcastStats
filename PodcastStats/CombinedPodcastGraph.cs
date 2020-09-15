using System.Collections.Generic;
using System.Linq;

namespace PodcastStats
{
    public class CombinedPodcastGraph
    {
        public List<PodcastStatInfo> Podcasts = new List<PodcastStatInfo>();

        public void Add(PodcastStatInfo podcastStatInfo)
        {
            this.Podcasts.Add(podcastStatInfo);
        }

        public IEnumerable<string> MakePlot()
        {
            var totalOutput = new List<PodcastStatInfoLine>();
            foreach (var p in this.Podcasts)
            {
                var podcastInfo = p.GetPodcastInfo();
                totalOutput.AddRange(podcastInfo);
            }

            var data = AggregateDataForPlot(totalOutput);

            return data;
        }
        
        private static IEnumerable<string> AggregateDataForPlot(List<PodcastStatInfoLine> outputs)
        {
            var allDates = outputs.Select(e => e.PublishDate).Distinct().ToList();
            var podcastTypes = outputs.Select(e => e.Podcast).Distinct().ToList();

            var header = podcastTypes.Aggregate("Date,", (current, p) => current + p + ",");

            var dataText = new List<string> {header};
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


            return dataText;
        }

    }
}