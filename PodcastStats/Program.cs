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
            
            string url = "https://feeds.99percentinvisible.org/";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            Console.WriteLine(feed.Items.Count());
            var output = new Dictionary<DateTimeOffset, string>();
            
            foreach (SyndicationItem item in feed.Items)
            {
                var publishDate = item.PublishDate;
                var durationStr = "";
                
                var durationEl = item.ElementExtensions.ReadElementExtensions<XmlElement>("duration", "http://www.itunes.com/dtds/podcast-1.0.dtd")[0];

                if (durationEl != null)
                {
                    durationStr = durationEl.InnerText;
                    
                    output.Add(item.PublishDate, durationStr);
                }
                String subject = item.Title.Text;    
                String summary = item.Summary.Text;
            }

            foreach (var d in output)
            {
                Console.WriteLine(d.Key.Date + ","  + d.Value);
            }
        }
    }
}