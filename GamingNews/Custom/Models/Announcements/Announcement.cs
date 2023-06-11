using System;

namespace GamingWeb.Custom.Models.Announcements
{
    public class Announcement
    {
        public int? Id { get; set; }
        public string TitleSq { get; set; }
        public string TitleEn { get; set; }
        public string TitleSr { get; set; }
        public string DescriptionSq { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionSr { get; set; }
        public string FileUrl { get; set; }
        public DateTime DisplayDate { get; set; }
        public int CreatedUserId { get; set; }
        public int UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
