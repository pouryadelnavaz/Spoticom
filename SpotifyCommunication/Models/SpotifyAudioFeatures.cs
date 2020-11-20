using System;
using System.Collections.Generic;
using System.Text;

namespace SpotifyCommunication.Models
{
    public class SpotifyAudioFeatures
    {
        private double _MinDancablity;
        public double MinDancablity
        {
            get { return _MinDancablity; }
            set
            {
                if (value < 0)
                    _MinDancablity = 0;
                else if (value > 1)
                    _MinDancablity = 1;
                else
                    _MinDancablity = value;
            }
        }

        private double _MaxDancablity;
        public double MaxDancablity
        {
            get { return _MaxDancablity; }
            set
            {
                if (value < 0)
                    _MaxDancablity = 0;
                else if (value > 1)
                    _MaxDancablity = 1;
                else
                    _MaxDancablity = value;
            }
        }

        private double _MinAcousticness;
        public double MinAcousticness
        {
            get { return _MinAcousticness; }
            set
            {
                if (value < 0)
                    _MinAcousticness = 0;
                else if (value > 1)
                    _MinAcousticness = 1;
                else
                    _MinAcousticness = value;
            }
        }

        private double _MaxAcousticness;
        public double MaxAcousticness
        {
            get { return _MaxAcousticness; }
            set
            {
                if (value < 0)
                    _MaxAcousticness = 0;
                else if (value > 1)
                    _MaxAcousticness = 1;
                else
                    _MaxAcousticness = value;
            }
        }

        private double _MinEnergy;
        public double MinEnergy
        {
            get { return _MinEnergy; }
            set
            {
                if (value < 0)
                    _MinEnergy = 0;
                else if (value > 1)
                    _MinEnergy = 1;
                else
                    _MinEnergy = value;
            }
        }

        private double _MaxEnergy;
        public double MaxEnergy
        {
            get { return _MaxEnergy; }
            set
            {
                if (value < 0)
                    _MaxEnergy = 0;
                else if (value > 1)
                    _MaxEnergy = 1;
                else
                    _MaxEnergy = value;
            }
        }

        private double _MinInstrumentalness;
        public double MinInstrumentalness
        {
            get { return _MinInstrumentalness; }
            set
            {
                if (value < 0)
                    _MinInstrumentalness = 0;
                else if (value > 1)
                    _MinInstrumentalness = 1;
                else
                    _MinInstrumentalness = value;
            }
        }

        private double _MaxInstrumentalness;
        public double MaxInstrumentalness
        {
            get { return _MaxInstrumentalness; }
            set
            {
                if (value < 0)
                    _MaxInstrumentalness = 0;
                else if (value > 1)
                    _MaxInstrumentalness = 1;
                else
                    _MaxInstrumentalness = value;
            }
        }
     

    }
    public class AudioFeature
    {
        public double danceability { get; set; }
        public double energy { get; set; }
        public int key { get; set; }
        public double loudness { get; set; }
        public int mode { get; set; }
        public double speechiness { get; set; }
        public double acousticness { get; set; }
        public double instrumentalness { get; set; }
        public double liveness { get; set; }
        public double valence { get; set; }
        public double tempo { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string uri { get; set; }
        public string track_href { get; set; }
        public string analysis_url { get; set; }
        public int duration_ms { get; set; }
        public int time_signature { get; set; }
    }

    public class SeveralTracksAudioFeature
    {
        public List<AudioFeature> audio_features { get; set; }
    }
}
