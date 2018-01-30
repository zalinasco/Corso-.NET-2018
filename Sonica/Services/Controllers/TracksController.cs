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
		public List<Track> Get(int PageNumber, int PageLenght)
		{
			return Indexer.List(PageNumber, PageLenght);
		}

		[HttpGet]
		[Route("{Key}")]
		public Track Get(string Key)
		{
			return Indexer.Track(Key);
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}
	}
}
