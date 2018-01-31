using Services.Classes;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Services.Controllers
{

	[RoutePrefix("api/tracks")]
	public class TracksController : ApiController
	{

		[HttpGet]
		[Route("")]
		public List<Track> Get(int PageNumber, int PageLenght = 10, string Album = null, string Artist = null)
		{
			return Indexer.List(PageNumber, PageLenght, Album, Artist);
		}

		[HttpGet]
		[Route("{Key}")]
		public Track Get(string Key)
		{
			return Indexer.Track(Key);
		}

	}
}
