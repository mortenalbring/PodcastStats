using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;


namespace PodcastStats
{
    internal class Program
    {

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
        }
        
        public static void Main(string[] args)
        {
            
            string url = "https://feeds.99percentinvisible.org/";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            Console.WriteLine(feed.Items.Count());
            var output2 = new List<PodcastStatInfo>();
            
            foreach (SyndicationItem item in feed.Items)
            {
                var durationStr = "";
                
                var durationEl = item.ElementExtensions.ReadElementExtensions<XmlElement>("duration", "http://www.itunes.com/dtds/podcast-1.0.dtd")[0];

                if (durationEl != null && durationEl.InnerText != "0:00")
                {
                    durationStr = durationEl.InnerText;
                    
                    var psi = new PodcastStatInfo();
                    psi.Duration = durationStr;
                    psi.PublishDate = item.PublishDate;
                    psi.Title = item.Title.Text;
                    psi.Id = item.Id;
                    psi.Podcast = "99% Invisible";
                    output2.Add(psi);
                }
            }

            // foreach (var d in output)
            // {
            //     Console.WriteLine(d.Key.Date + ","  + d.Value);
            // }

            foreach (var d in output2)
            {
                Console.WriteLine(d.MakeStatLine());
            }
        }
    }
}