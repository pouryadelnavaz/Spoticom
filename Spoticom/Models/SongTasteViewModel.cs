using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spoticom.Models
{
    public class SongTasteViewModel
    {
        public string URL { get; set; }
        public bool Check { get; set; }
        public string ArtistName { get; set; }
        public string SongName { get; set; }
        public string SpotifyURL { get; set; }
        public string id { get; set; }
    }
}