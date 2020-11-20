using SpotifyCommunication.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpotifyCommunication
{
    public class SpotifyApiLibrary : HTTPClient
    {
        private string _AccessToken;
        public SpotifyApiLibrary(string AccessToken)
        {
            _AccessToken = AccessToken;
        }
        public async Task<SpotifyRecommendationObject> GetRecommendationsAsync(string ArtistsIds, string Genres,string TracksIds, string Market, int Limit)
        {
            var content = $"?limit={Limit}&market={Market}&seed_artists={ArtistsIds}&seed_genres={Genres}&seed_tracks={TracksIds}";
            var Recoms = await SendGetRequestAsync<SpotifyRecommendationObject>(true, "recommendations",_AccessToken,content);
            return Recoms;
        }
        public async Task<SeveralTracksAudioFeature> GetAudioFeaturesForSeveralTracks(string TrackIds)
        {
            var content = $"?ids={TrackIds}";
            var TracksFeature = await SendGetRequestAsync<SeveralTracksAudioFeature>(true, "audio-features", _AccessToken, content);
            return TracksFeature;
        }
        public async Task<SpotifyTrackObject> GetSeveralTracks(string TrackIds)
        {
            var content = $"?ids={TrackIds}";
            var SpotifyTracks = await SendGetRequestAsync<SpotifyTrackObject>(true, "tracks", _AccessToken, content);
            return SpotifyTracks;
        }
        public async Task<SpotifyRecommendationObject> GetRecommendationsAsync(string ArtistsIds, string Genres, string TracksIds, string Market,int Limit, SpotifyAudioFeatures AudioFeature)
        {
            var content = $"?limit={Limit}&market={Market}&seed_artists={ArtistsIds}&seed_genres={Genres}&seed_tracks={TracksIds}&min_acousticness={AudioFeature.MinAcousticness}&max_acousticness={AudioFeature.MaxAcousticness}&min_danceability={AudioFeature.MinDancablity}&max_danceability={AudioFeature.MaxDancablity}&min_energy={AudioFeature.MinEnergy}&max_energy={AudioFeature.MaxEnergy}&min_instrumentalness={AudioFeature.MinInstrumentalness}&max_instrumentalness={AudioFeature.MaxInstrumentalness}";
            var Recom = await SendGetRequestAsync<SpotifyRecommendationObject>(true, "recommendations", _AccessToken, content);
            return Recom;
        }
        public async Task<SpotifyUserObject> GetCurrentUserInfoAsync()
        {
            var UserObject = await SendGetRequestAsync<SpotifyUserObject>(true, "me",_AccessToken);
            return UserObject;
        }
        public async Task<SpotifyTrackObject> GetArtistTopSongsAsync(string ArtistId, string Market)
        {
            var content = $"/{ArtistId}/top-tracks?market={Market}";
            var Songs = await SendGetRequestAsync<SpotifyTrackObject>(true, "artists", _AccessToken, content);
            return Songs;
        }
        public async Task<RelatedArtistObject> GetArtistsRelatedAsync(string ArtistId)
        {
            var content = $"/{ArtistId}/related-artists";
            var SearchArtistObjects = await SendGetRequestAsync<RelatedArtistObject>(true, "artists",_AccessToken, content);
            return SearchArtistObjects;
        }
        public async Task<SpotifySearchResponseArtistObject> SearchAsync(SpotifySearchRequestObject ArtistSearchObject)
        {
            var content = $"?q={ArtistSearchObject.q}&type={ArtistSearchObject.type}";
            var SearchArtistObjects = await SendGetRequestAsync<SpotifySearchResponseArtistObject>(true, "search",_AccessToken, content);
            return SearchArtistObjects;
        }
       
    }
}
