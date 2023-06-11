using System.Collections.Generic;

namespace GamingWeb.Custom.Models.Directory
{
    public class GeneralDirector
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionSq { get; set; }
        public string DescriptionSr { get; set; }
        public string FileUrl { get; set; }
        public string FbUrl { get; set; }
        public string TwUrl { get; set; }
        public string LiUrl { get; set; }
        public int UpdatedUserId { get; set; }
    }
}
