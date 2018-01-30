using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Services.Models
{
	public class Track
	{

		[XmlIgnore]
		[ScriptIgnore]
		public string FilePath
		{
			get; set;
		}

		public string Key
		{
			get; set;
		}

		public string Title
		{
			get; set;
		}

		public string Album
		{
			get; set;
		}

		public string Artist
		{
			get; set;
		}

		public uint Number
		{
			get; set;
		}

		public long Duration
		{
			get; set;
		}


	}


}