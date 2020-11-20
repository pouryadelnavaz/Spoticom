using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spoticom.Models
{
    public class RecommendationViewModel
    {
        public string Id { get; set; }
        public string EmbedUrl { get; set; }
        public SpotifyCommunication.Models.Track Track { get; set; }
    }
}