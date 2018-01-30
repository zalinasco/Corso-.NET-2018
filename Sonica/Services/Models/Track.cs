using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class Track
    {
        public string FilePath
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

        public int TrackNumber
        {
            get; set;
        }

        public int Duration
        {
            get; set;
        }


    }


}