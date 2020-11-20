using System;
using System.Collections.Generic;
using System.Text;

namespace SpotifyCommunication.Models
{
    public class SpotifyRecommendationObject
    {
        public IList<Track> tracks { get; set; }
        public IList<Seed> seeds { get; set; }
    }
    public class Seed
    {
        public int initialPoolSize { get; set; }
        public int afterFilteringSize { get; set; }
        public int afterRelinkingSize { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string href { get; set; }
    }
    public class LinkedFrom
    {
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
