using System;
using System.Collections.Generic;

namespace Forte.Svc.Services.Models.Blobs
{
    public class Blob
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Container { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UploadedBy { get; set; }

        public IDictionary<string, string> Metadata { get; set; }

        public string Url { get; set; }
    }
}
