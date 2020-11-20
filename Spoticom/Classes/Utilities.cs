using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using SpotifyCommunication;
using SpotifyCommunication.Models;
using Spoticom.Models;
using Newtonsoft.Json;

namespace Spoticom.Classes
{
    public static class Utilities
    {
        public static AuthorizationModel GetAuthorizationInfo()
        {
            var AuthorizationModel = new AuthorizationModel()
            {
                ClientID = WebConfigurationManager.AppSettings["SpotifyAppClientID"],
                ClientSecret = WebConfigurationManager.AppSettings["SpotifyAppClientSecret"]
            };
            return AuthorizationModel;
        }
        public static string GetSpotifyEmbedTrackUrl(string TrackId)
        {
            return $"https://open.spotify.com/embed/track/{TrackId}";
        }
        public static string GetTrackIds(IEnumerable<Track> Tracks)
        {
            var trackIds = new List<string>();
            foreach (var song in Tracks)
            {
                trackIds.Add(song.id);
            }
            return string.Join(",", trackIds.ToArray());
        }
        public static async Task<List<RecommendationViewModel>> GetRecommendationsByTracksAsync(List<Track> Tracks, SpotifyApiLibrary Client, string Market, SpotifyAudioFeatures AudioFeatures)
        {
            var FinalRecommendation = new List<RecommendationViewModel>();
            foreach (var RecomTrack in Tracks)
            {
                try
                {
                    var RecomArtist = RecomTrack.artists.First();
                    var relatedArtists = await Client.GetArtistsRelatedAsync(RecomArtist.id);
                    relatedArtists.artists.Shuffle();
                    var ItemRelatedArtist = relatedArtists.artists.First();
                    var TopSong = await Client.GetArtistTopSongsAsync(ItemRelatedArtist.id, Market);
                    TopSong.tracks.Shuffle();
                    var Top20 = TopSong.tracks.Take(20);
                    var FinalSongIds = GetTrackIds(Top20.Take(100));
                    var SpotifySeveralTracksAudioFeatures = await Client.GetAudioFeaturesForSeveralTracks(FinalSongIds);
                    var filtered = FilterAudioFeatures(SpotifySeveralTracksAudioFeatures.audio_features, AudioFeatures);
                    FinalSongIds = GetTrackIds(filtered);
                    var FinalRelatedArtistTracksFilteredResult = await Client.GetSeveralTracks(FinalSongIds);
                    var TheMostPopularTrack = FinalRelatedArtistTracksFilteredResult.tracks.OrderBy(x => x.popularity).FirstOrDefault();
                    if (TheMostPopularTrack != null)
                    {
                        FinalRecommendation.Add(new RecommendationViewModel { Id = TheMostPopularTrack.id, EmbedUrl = GetSpotifyEmbedTrackUrl(TheMostPopularTrack.id) });
                    }
                }
                catch (Exception e)
                {
                    break;
                }

            }
            return FinalRecommendation;
        }
        public static IEnumerable<AudioFeature> FilterAudioFeatures(List<AudioFeature> Tracks, SpotifyAudioFeatures AudioFeatures)
        {
            return Tracks.Where(x =>
                            (x.acousticness >= AudioFeatures.MinAcousticness && x.acousticness <= AudioFeatures.MaxAcousticness) &&
                            (x.danceability >= AudioFeatures.MinDancablity && x.danceability <= AudioFeatures.MaxDancablity) &&
                            (x.energy >= AudioFeatures.MinEnergy && x.energy <= AudioFeatures.MaxEnergy) &&
                            (x.instrumentalness >= AudioFeatures.MinInstrumentalness && x.instrumentalness <= AudioFeatures.MaxInstrumentalness));
        }
        public static string GetTrackIds(IEnumerable<AudioFeature> Tracks)
        {
            var trackIds = new List<string>();
            foreach (var song in Tracks)
            {
                trackIds.Add(song.id);
            }
            return string.Join(",", trackIds.ToArray());
        }
        public static List<Model.ISO3166Alpha2Model> GetMarkets()
        {
            var FilePath = HttpContext.Current.Server.MapPath("~/Models/ISO3166-1.alpha2.json");
            var json = File.ReadAllText(FilePath);
            var Markets = JsonConvert.DeserializeObject<List<Model.ISO3166Alpha2Model>>(json);
            return Markets;
        }
        public static SpotifyAudioFeatureRangeModelInt GetIntegerRange(string Range)
        {
            int[] ArrayRange = Range.Split(',').Select(int.Parse).ToArray();
            return new SpotifyAudioFeatureRangeModelInt { Min = ArrayRange.Min(), Max = ArrayRange.Max() };
        }
        public static SpotifyAudioFeatureRangeModelDouble GetDoubleRange(string Range)
        {
            double[] ArrayRange = Range.Split(',').Select(double.Parse).ToArray();
            return new SpotifyAudioFeatureRangeModelDouble { Min = ArrayRange.Min(), Max = ArrayRange.Max() };
        }
    }
}