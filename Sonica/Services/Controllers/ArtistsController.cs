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

	[RoutePrefix("api/artists")]
	public class ArtistaController : ApiController
	{

    [HttpOptions]
    [Route("{*all}")]
    public HttpResponseMessage Options(string All)
    {
      return new HttpResponseMessage(HttpStatusCode.OK);
    }

    [HttpGet]
		[Route("")]
		public List<string> Get()
		{
			return Indexer.ListArtists();
		}

	}
}
