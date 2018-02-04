using Services.Classes;
using Services.Classes.Helpers;
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

    [HttpOptions]
    [Route("{*all}")]
    public HttpResponseMessage Options(string All)
    {
      return new HttpResponseMessage(HttpStatusCode.OK);
    }

    [HttpGet]
    [Route("")]
		public List<Track> Get(int PageNumber=0, int PageLength = 10, string Album = null, string Artist = null)
		{
			return Indexer.List(PageNumber, PageLength, Album, Artist);
		}

    [HttpGet]
    [Route("{Key}")]
    public Track Get(string Key)
    {
      return Indexer.Track(Key);
    }

    [HttpGet]
    [Route("{Key}/stream")]
    public HttpResponseMessage GetStream(string Key)
    {
      return new FileStreamResponse(Indexer.Track(Key).FilePath);
    }

  }
}
