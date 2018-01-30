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
        private static List<Track> listaMp3 = new List<Track>();
        public static void CreateIndex()
        {
            string root_index = ConfigurationManager.AppSettings["libraryRoot"];
            if (!Directory.Exists(root_index)){
                Directory.CreateDirectory(root_index);
            }
            listaMp3.AddRange(
                Directory
                .EnumerateFiles(root_index, "*.mp3")
                .Select(o => new Track()
                {
                    filepath = o
                })

                );
        }
    }
}