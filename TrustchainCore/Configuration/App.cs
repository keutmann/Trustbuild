using Newtonsoft.Json.Linq;
using System;
using System.IO;
using TrustchainCore.IO;

namespace TrustchainCore.Configuration
{
    public abstract class App
    {
        public static JObject Config = new JObject();

        static App()
        {
        }

        public static void EnableEventLogger()
        {
            Console.SetOut(new EventLoggerTextWriter(Console.Out));
            Console.SetError(new ErrorEventLoggerTextWriter(Console.Error));
        }

        public static void LoadConfigFile(string filename)
        {
            if (File.Exists(filename))
            {
                var text = File.ReadAllText(filename);
                Config.Merge(JObject.Parse(text));
            }
        }

        public static void SaveConfigFile(string filename)
        {
            var json = Config.ToString();
            File.WriteAllText(filename, json);
        }
    }
}
