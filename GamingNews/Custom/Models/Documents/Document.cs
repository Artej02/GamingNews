using System;

namespace GamingWeb.Custom.Models.Documents
{
    public class Document
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
