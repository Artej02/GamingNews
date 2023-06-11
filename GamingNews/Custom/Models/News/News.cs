using System;

namespace GamingWeb.Custom.Models.News
{
    public class News
    {
        public int? Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleSq { get; set; }
        public string TitleSr { get; set; }
        public string ContentEn { get; set; }
        public string ContentSq { get; set; }
        public string ContentSr { get; set; }
        public DateTime NewsDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Expire { get; set; }
        public DateTime ExpireDate { get; set; }
        public int? DepartmentId { get; set; }
        public string Photo { get; set; }
        public string Place { get; set; }
        public string FileUrl { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
