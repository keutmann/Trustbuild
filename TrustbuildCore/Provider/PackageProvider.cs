using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TrustbuildCore.Service;
using TrustchainCore.Data;
using TrustchainCore.Extensions;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Provider
{
    public class PackageProvider
    {
        public string CurrentPackageName { get; set; }

        public PackageProvider(string currentPackageName)
        {
            CurrentPackageName = currentPackageName;
        }

        public IEnumerable<string> GetBuildPackages()
        {
            return GetBuildPackages(App.Config["buildpath"].ToStringValue());
        }

        public IEnumerable<string> GetBuildPackages(string buildFolderPath)
        {
            var names = from p in Directory.EnumerateFiles(buildFolderPath)
                        where !CurrentPackageName.Equals(p, StringComparison.OrdinalIgnoreCase)
                        select p;

            return names;
        }


        public List<string> GetBuildPackages(WorkflowStatus status)
        {
            var names = GetBuildPackages();
            var list = new List<string>();
            foreach (var name in names)
            {
                using (var db = TrustchainDatabase.Open(name))
                {
                    var json = db.KeyValue.Get("state");
                    var state = JsonConvert.DeserializeObject<WorkflowState>(json);
                    if (state.Status == status)
                        list.Add(name);
                }
            }
            return list;
        }
    }


}
