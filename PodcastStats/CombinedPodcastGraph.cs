using System.Collections.Generic;
using System.Linq;

namespace PodcastStats
{
    public class CombinedPodcastGraph
    {
        private readonly List<PodcastStatInfo> Podcasts = new List<PodcastStatInfo>();

        public void Add(PodcastStatInfo podcastStatInfo)
        {
            Podcasts.Add(podcastStatInfo);
        }

        public IEnumerable<string> MakePlot()
        {
            var totalPodcastEpisodeInfos = new List<PodcastEpisodeInfo>();
            foreach (var p in Podcasts)
            {
                p.GetPodcastInfo();
                totalPodcastEpisodeInfos.AddRange(p.PodcastStatInfoLines);
            }

            var data = AggregateDataForPlot(totalPodcastEpisodeInfos);

            return data;
        }

        /// <summary>
        ///     Aggregates data from all the podcasts specified and outputs data
        ///     <remarks>This method is quite inefficient, can be improved</remarks>
        /// </summary>
        /// <param name="episodeInfoLines">Podcast episode info lines</param>
        /// <returns>List of comma separated data lines</returns>
        private static IEnumerable<string> AggregateDataForPlot(List<PodcastEpisodeInfo> episodeInfoLines)
        {
            var allDates = episodeInfoLines.Select(e => e.PublishDate).Distinct().ToList();
            var podcastTypes = episodeInfoLines.Select(e => e.Podcast).Distinct().ToList();

            var header = podcastTypes.Aggregate("Date,", (current, p) => current + p + ",");

            var dataText = new List<string> {header};
            foreach (var d in allDates)
            {
                var outputStr = d.ToString("yyyy/MM/dd");
                foreach (var p in podcastTypes)
                {
                    var dateDur = "";

                    var match = episodeInfoLines.Where(e => e.PublishDate == d && e.Podcast == p).ToList();
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