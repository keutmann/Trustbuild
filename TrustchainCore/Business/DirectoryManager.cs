using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustchainCore.Business
{

    public class DirectoryManager
    {

        public string AppData { get; set; }



        public DirectoryManager()
        {
            AppData = "%appdata%\\Trustchain\\";
        }
        

        public string GetLibraryFolder()
        {
            return Path.Combine(AppData, "Library");
        }

        public string GetTorrentFolder()
        {
            return Path.Combine(AppData, "Torrent");
        }

    }
}
