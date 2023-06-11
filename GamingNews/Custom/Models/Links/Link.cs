using System;

namespace GamingWeb.Custom.Models.Links
{
    public class Link
    {
        public int? Id { get; set; }
        public string TitleSq { get; set; }
        public string TitleEn { get; set; }
        public string TitleSr { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int CreatedUserId { get; set; }
        public int UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
