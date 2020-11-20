using System;
using System.Collections.Generic;
using System.Text;

namespace SpotifyCommunication.Models
{
    public class Item
    {
        public ExternalUrls external_urls { get; set; }
        public Followers followers { get; set; }
        public IList<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public IList<Image> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Artists
    {
        public string href { get; set; }
        public IList<Item> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class RelatedArtistObject
    {
        public IList<Artist> artists { get; set; }
    }
    public class Artist
    {
        public ExternalUrls external_urls { get; set; }
        public Followers followers { get; set; }
        public IList<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public IList<Image> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class SpotifySearchResponseArtistObject
    {
        public Artists artists { get; set; }
    }
}
