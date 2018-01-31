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

	[RoutePrefix("api/albums")]
	public class AlbumsController : ApiController
	{

		[HttpGet]
		[Route("")]
		public List<string> Get()
		{
			return Indexer.ListAlbums();
		}

	}
}
