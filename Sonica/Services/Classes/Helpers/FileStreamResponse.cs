using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Services.Classes.Helpers
{
  public class FileStreamResponse : HttpResponseMessage
  {
    public FileStreamResponse(string FilePath)
    {
      try
      {
        Content = new StreamContent(File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read));
        Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(FilePath));
        Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
        Content.Headers.ContentDisposition.FileName = Path.GetFileName(FilePath);
      }
      catch (Exception Ex)
      {
      }
    }

  }
}