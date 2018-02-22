using Newtonsoft.Json;
using Services.Classes.Extensions;
using Services.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;

namespace Services.Classes
{
  public class Indexer
  {
    private static Dictionary<string, Track> _Data = new Dictionary<string, Track>();
    private static string _LibraryRoot;
    private static string _IndexFilePath;
    private static Thread _MaintenanceThread = null;

		private static Tuple<DateTime, TokenResponse> TokenStorage = new Tuple<DateTime, TokenResponse>(DateTime.MinValue, null);

		internal static readonly HttpClient tokenRequestClient = new HttpClient();
		internal static readonly HttpClient apiClient = new HttpClient();

		private const string _TokenRequestURL = "https://accounts.spotify.com/api/token";
		private const string _SearchURLTemplate = "https://api.spotify.com/v1/search?q={0}&type={1}";

		#region Library maintenance

		public static void Init()
    {

      _LibraryRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["LibraryRoot"]);

      if (!Directory.Exists(_LibraryRoot))
      {
        Directory.CreateDirectory(_LibraryRoot);
      }

      _IndexFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index.json");

      if (!File.Exists(_IndexFilePath))
      {
        Persist();
      }

			var byteArray = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ClientID"] + ":" + ConfigurationManager.AppSettings["ClientSecret"]);
			tokenRequestClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

			Load();

      EnsureThreads();

    }

    private static void MaintainIndex()
    {

      Remove();
      Add();
      Persist();

    }

    private static void Persist()
    {

      File.WriteAllText(_IndexFilePath, JsonConvert.SerializeObject(_Data));

    }

    private static void Load()
    {

      _Data = JsonConvert.DeserializeObject<Dictionary<string, Track>>(File.ReadAllText(_IndexFilePath));

    }

    private static void Add()
    {

      foreach (string fn in Directory.EnumerateFiles(_LibraryRoot, "*.mp3", SearchOption.AllDirectories))
      {
        string Key = fn.Replace(_LibraryRoot, string.Empty).Base64Encode();

        if (!_Data.ContainsKey(Key))
        {
          _Data
            .Add
            (
              Key,
              GetTrack(fn)
            );
        }
      }

    }

    private static void Remove()
    {

      foreach (string k in _Data.Keys.ToList())
      {
        if (!File.Exists(_Data[k].FilePath))
        {
          _Data.Remove(k);
        }
      }

    }

		private static void EnsureToken()
		{
			if (TokenStorage.Item1 > DateTime.Now)
			{
				return;
			}

			var tokenRequestArguments = new Dictionary<string, string>
			{
				 { "grant_type", "client_credentials" }
			};

			var tokenRequestContent = new FormUrlEncodedContent(tokenRequestArguments);

			var tokenRequestResponse = tokenRequestClient.PostAsync(_TokenRequestURL, tokenRequestContent).Result;

			var tokenRequestResponseString = tokenRequestResponse.Content.ReadAsStringAsync().Result;

			TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(tokenRequestResponseString);

			DateTime expire = DateTime.Now.AddSeconds(token.expires_in);

			TokenStorage = new Tuple<DateTime, TokenResponse>(expire, token);

		}

		private static Models.SearchResponse.Response SearchArtist(string Artist)
		{
			EnsureToken();

			apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenStorage.Item2.token_type, TokenStorage.Item2.access_token);

			var searchRequestResponse = apiClient.GetAsync(string.Format(_SearchURLTemplate, Uri.EscapeDataString(Artist), "artist")).Result;

			var searchRequestResponseString = searchRequestResponse.Content.ReadAsStringAsync().Result;

			return JsonConvert.DeserializeObject<Models.SearchResponse.Response>(searchRequestResponseString);
		}

		#endregion

		#region Threads

		private static void EnsureThreads()
    {
      if (_MaintenanceThread != null)
      {
        int t = 0;
        _MaintenanceThread.Abort();
        while (_MaintenanceThread.ThreadState != System.Threading.ThreadState.Aborted && t < 10)
        {
          t++;
          Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));
        }
      }

      if (_MaintenanceThread == null || _MaintenanceThread.ThreadState == System.Threading.ThreadState.Aborted)
      {
        _MaintenanceThread = new Thread(new ThreadStart(MaintainLibrary));
        _MaintenanceThread.IsBackground = true;
        _MaintenanceThread.Name = "Maintenance";
        _MaintenanceThread.Start();
      }
      else
      {
        throw new Exception("Unable to start TimerThread because it's not in the right state.");
      }

    }

    private static void MaintainLibrary()
    {
      while (true)
      {

        try
        {
          MaintainIndex();
        }
        catch (Exception Ex)
        {
        }

        Thread.Sleep(new TimeSpan(0, 5, 0));
      }
    }

    #endregion

    #region User interaction

    internal static List<Track> List(int PageNumber, int PageLength, string Album, string Artist)
    {

			Func<uint> Random = () => 
			{
				Random rnd = new Random();
				return (uint)rnd.Next();
			};


      return
        _Data
        .Values
        .Where
        (
          o =>
          (string.IsNullOrEmpty(Album) || o.Album == Album)
          &&
          (string.IsNullOrEmpty(Artist) || o.Artist == Artist)
        )
        .OrderBy
        (
          o =>
          (string.IsNullOrEmpty(Album) ? Random() : o.Number)
        )
        .Skip(PageLength * PageNumber)
        .Take(PageLength)
        .ToList();

    }

    internal static Track Track(string Key)
    {

      return _Data[Key];

    }

		private static Dictionary<string, Models.SearchResponse.Response> ArtistsCache = new Dictionary<string, Models.SearchResponse.Response>();

    private static Track GetTrack(string FilePath)
    {

      Track r = new Track()
      {
        FilePath = FilePath
      };

      var tagReader = TagLib.File.Create(FilePath);

      r.Key = FilePath.Replace(_LibraryRoot, string.Empty).Base64Encode();
      r.Album = tagReader.Tag.Album;
      r.Artist = tagReader.Tag.AlbumArtists.FirstOrDefault() ?? "N/A";
      r.Title = tagReader.Tag.Title;
      r.Number = tagReader.Tag.Track;
      r.Duration = tagReader.Length;

			Models.SearchResponse.Response artists;

			if (!ArtistsCache.ContainsKey(r.Artist))
			{
				artists = SearchArtist(r.Artist);
				ArtistsCache.Add(r.Artist, artists);
			}
			else
			{
				artists = ArtistsCache[r.Artist];
			}

			if(artists.artists.total > 0)
			{
				r.BackdropImageURL = artists.artists.items[0].images.OrderBy(o => o.width).Select(o => o.url).FirstOrDefault();
			}

			return r;

    }

    internal static List<string> ListAlbums()
    {

      return
        _Data
        .Values
        .Select
        (
          o => o.Album
        )
        .Distinct()
        .ToList();

    }

    internal static List<string> ListArtists()
    {

      return
        _Data
        .Values
        .Select
        (
          o => o.Artist
        )
        .Distinct()
        .ToList();

    }

    #endregion

  }
}