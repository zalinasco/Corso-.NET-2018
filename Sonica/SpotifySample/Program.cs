using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpotifySample
{
	class Program
	{

		private static Tuple<DateTime, TokenResponse> TokenStorage = new Tuple<DateTime, TokenResponse>(DateTime.MinValue, null);

		internal static readonly HttpClient tokenRequestClient = new HttpClient();
		internal static readonly HttpClient apiClient = new HttpClient();

		private const string _TokenRequestURL = "https://accounts.spotify.com/api/token";
		private const string _SearchURLTemplate = "https://api.spotify.com/v1/search?q={0}&type={1}";

		static Program()
		{
			var byteArray = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ClientID"] + ":" + ConfigurationManager.AppSettings["ClientSecret"]);
			tokenRequestClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
		}

		static void Main(string[] args)
		{

			for(int i = 0; i<10; i++)
			{ 
				PerformSearch();
			}
		}

		private static void PerformSearch()
		{
			EnsureToken();

			apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenStorage.Item2.token_type, TokenStorage.Item2.access_token);

			var searchRequestResponse = apiClient.GetAsync(string.Format(_SearchURLTemplate, "ACDC", "artist")).Result;

			var searchRequestResponseString = searchRequestResponse.Content.ReadAsStringAsync().Result;

			SearchResponse.Response searchresponse = JsonConvert.DeserializeObject<SearchResponse.Response>(searchRequestResponseString);
		}

		private static void EnsureToken()
		{
			if(TokenStorage.Item1 > DateTime.Now)
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
	}
}
