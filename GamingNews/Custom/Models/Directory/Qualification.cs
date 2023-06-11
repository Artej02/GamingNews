using System;

namespace GamingWeb.Custom.Models.Directory
{
    public class Qualification
    {
        public int? Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleSq { get; set; }
        public string TitleSr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionSq { get; set; }
        public string DescriptionSr { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }
}
