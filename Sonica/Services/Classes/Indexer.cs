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

		public static void Init()
		{

			string _LibraryRoot = ConfigurationManager.AppSettings["LibraryRoot"];
			if (!Directory.Exists(_LibraryRoot))
			{
				Directory.CreateDirectory(_LibraryRoot);
			}

		}

		private static void MaintainIndex()
		{

			Load();
			Remove();
			Add();
			Persist();

		}

		private static void Add()
		{

			foreach(string fn in Directory.EnumerateFiles(_LibraryRoot, "*.mp3", SearchOption.AllDirectories))
			{
				if(!_Data.ContainsKey(fn))
				{

				}
			}

		}

		private static void Remove()
		{
			
			foreach(string k in _Data.Keys.ToList())
			{
				if(!File.Exists(k))
				{
					_Data.Remove(k);
				}
			}

		}

	}
}