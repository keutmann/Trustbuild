using MonoTorrent;
using MonoTorrent.Common;
using System;
using System.IO;
using TrustbuildCore.Business;
using TrustchainCore.Business;

namespace TrustbuildCore.Workflow
{
    public class BuildTorrentWorkflow : WorkflowPackage
    {
        public override void Execute()
        {
            GC.Collect(); // Release old connections, to make sure that the file is not locked

            //var name = new FileInfo(Package.Filename).Name;
            var tp = AppDirectory.TorrentPath;
            var savePath = Path.Combine(tp, Package.Filename).Replace(".db", ".torrent");
            CreateTorrent(Package.FilePath, savePath);

            Context.Log("Package torrent created");
            Context.Enqueue(typeof(PublishPackageWorkflow));
        }

        public void CreateTorrent(string path, string savePath)
        {
            // The class used for creating the torrent
            TorrentCreator c = new TorrentCreator();

            // Add one tier which contains two trackers
            //RawTrackerTier tier = new RawTrackerTier();
            //tier.Add("http://localhost/announce");

            //c.Announces.Add(tier);
            c.Comment = "This is a Trust package";
            c.CreatedBy = ServerIdentity.Current.Address.ToWif();
            c.Publisher = "Trustchain";
            c.PieceLength = 64 * 1024;


            // Set the torrent as private so it will not use DHT or peer exchange
            // Generally you will not want to set this.
            c.Private = false;

            // Every time a piece has been hashed, this event will fire. It is an
            // asynchronous event, so you have to handle threading yourself.
            c.Hashed += delegate (object o, TorrentCreatorEventArgs e)
            {
                //Console.WriteLine("Current File is {0}% hashed", e.FileCompletion);
                //Console.WriteLine("Overall {0}% hashed", e.OverallCompletion);
                //Console.WriteLine("Total data to hash: {0}", e.OverallSize);
            };

            //c.SetCustom("Tove", "Hans");

            // ITorrentFileSource can be implemented to provide the TorrentCreator
            // with a list of files which will be added to the torrent metadata.
            // The default implementation takes a path to a single file or a path
            // to a directory. If the path is a directory, all files will be
            // recursively added


            Check.SavePath(savePath);

            TorrentFileSource fileSource = new TorrentFileSource(path);
            
            // Create the torrent file and save it directly to the specified path
            // Different overloads of 'Create' can be used to save the data to a Stream
            // or just return it as a BEncodedDictionary (its native format) so it can be
            // processed in memory
            c.Create(fileSource, savePath);
        }
    }
}
