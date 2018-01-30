using Newtonsoft.Json;
using Services.Classes.Extensions;
using Services.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Services.Classes
{
	public class Indexer
	{
		private static Dictionary<string, Track> _Data = new Dictionary<string, Track>();
		private static string _LibraryRoot;
		private static string _IndexFilePath;

		internal static List<Track> List(int PageNumber, int PageLenght)
		{

			return
				_Data
				.Values
				.Skip(PageLenght * PageNumber)
				.Take(PageLenght)
				.ToList();

		}

		internal static Track Track(string Key)
		{

			return _Data[Key];

		}

		public static void Init()
		{

			_LibraryRoot = ConfigurationManager.AppSettings["LibraryRoot"];
			if (!Directory.Exists(_LibraryRoot))
			{
				Directory.CreateDirectory(_LibraryRoot);
			}

			_IndexFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index.json");

			if(!File.Exists(_IndexFilePath))
			{
				Persist();
			}

			Load();

			MaintainIndex();

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

			foreach(string fn in Directory.EnumerateFiles(_LibraryRoot, "*.mp3", SearchOption.AllDirectories))
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
			
			foreach(string k in _Data.Keys.ToList())
			{
				if(!File.Exists(_Data[k].FilePath))
				{
					_Data.Remove(k);
				}
			}

		}

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

			return r;

		}

	}
}