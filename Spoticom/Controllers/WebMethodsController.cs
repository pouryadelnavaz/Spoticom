using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Mvc;
using SpotifyCommunication.Models;
using System.Threading.Tasks;
using System.Net;
using Spoticom.Classes;
using Spoticom.Models;

namespace Spoticom.Controllers
{
    public class WebMethodsController : Controller
    {
        public JavaScriptSerializer Serializer = new JavaScriptSerializer();

        private string GetAccessTokenCookie()
        {
            HttpCookie AccessTokenCookie = Request.Cookies["SpotifyAccessToken"];
            if (AccessTokenCookie != null)
            {
                return AccessTokenCookie.Value;
            }
            else
            {
                return null;
            }
        }
        // GET: WebMethods
        [HttpGet]
        public ActionResult GetMarkets()
        {
            return Content(Serializer.Serialize(Utilities.GetMarkets()));
        }
        [HttpGet]
        public async Task<ActionResult> GetRecommandationWithAudioFeatures(string ArtistIds, string Genre, string TracksIds, string Market, string Dancablity, string Acousticness, string Energy,
            string Instrumentalness)
        {
            var Token = GetAccessTokenCookie(); //The SpotifyAuthenticationComponent middleware, always checks for having cookie in the app pipeline! 
            var Client = new SpotifyCommunication.SpotifyApiLibrary(Token);
            var AudioFeatures = new SpotifyAudioFeatures()
            {
                MaxAcousticness = Utilities.GetDoubleRange(Acousticness).Max,
                MinAcousticness = Utilities.GetDoubleRange(Acousticness).Min,
                MaxDancablity = Utilities.GetDoubleRange(Dancablity).Max,
                MinDancablity = Utilities.GetDoubleRange(Dancablity).Min,
                MaxEnergy = Utilities.GetDoubleRange(Energy).Max,
                MinEnergy = Utilities.GetDoubleRange(Energy).Min,
                MaxInstrumentalness = Utilities.GetDoubleRange(Instrumentalness).Max,
                MinInstrumentalness = Utilities.GetDoubleRange(Instrumentalness).Min,
            };
            var Recoms = await Client.GetRecommendationsAsync(ArtistIds, Genre, TracksIds, Market, 25, AudioFeatures);
            var ViewModel = Recoms.tracks.Select(x => new RecommendationViewModel()
            {
                Id = x.id,
                EmbedUrl = Utilities.GetSpotifyEmbedTrackUrl(x.id)
            }).ToList();
            return Content(Serializer.Serialize(ViewModel));
        }
        [HttpGet]
        public async Task<ActionResult> GetTastesTestSongs(string ArtistIds, string Market)
        {
            var Token = GetAccessTokenCookie(); //The SpotifyAuthenticationComponent middleware, always checks for having cookie in the app pipeline! 
            var Client = new SpotifyCommunication.SpotifyApiLibrary(Token);
            var ArtistsIdsArray = ArtistIds.Split(',');
            var ListArtistsTopSongs = new List<Track>();
            foreach (var ArtistId in ArtistsIdsArray)
            {
                var ArtistTopSong = await Client.GetArtistTopSongsAsync(ArtistId, Market);
                ListArtistsTopSongs.AddRange(ArtistTopSong.tracks);
            }
            var TasteList = ListArtistsTopSongs.OrderBy(x => x.popularity).Select(x => new SongTasteViewModel()
            {

                Check = false,
                URL = x.preview_url,
                ArtistName = x.artists.First().name,
                SongName = x.name,
                SpotifyURL = Utilities.GetSpotifyEmbedTrackUrl(x.id),
                id = x.id
            });
            return Content(Serializer.Serialize(TasteList));
        }
        [HttpGet]
        public async Task<ActionResult> SuggestArtist(string ArtistId)
        {
            var Token = GetAccessTokenCookie(); //The SpotifyAuthenticationComponent middleware, always checks for having cookie in the app pipeline! 
            var Client = new SpotifyCommunication.SpotifyApiLibrary(Token);
            var result = await Client.GetArtistsRelatedAsync(ArtistId);
            return Content(Serializer.Serialize(result));
        }
        [HttpGet]
        public async Task<ActionResult> SearchArtist(string Query)
        {
            SpotifySearchRequestObject searchObject = new SpotifySearchRequestObject()
            {
                q = Query,
                type = "artist"
            };
            var Token = GetAccessTokenCookie(); //The SpotifyAuthenticationComponent middleware, always checks for having cookie in the app pipeline! 
            var Client = new SpotifyCommunication.SpotifyApiLibrary(Token);
            var result = await Client.SearchAsync(searchObject);
            try
            {
                return Content(Serializer.Serialize(result));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }
    }
}