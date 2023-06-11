using System;

namespace GamingWeb.Custom.Models.User
{
    public class Role
    {
        public int? Id { get; set; }
        public string NameEn { get; set; }
        public string NameSq { get; set; }
        public string NameSr { get; set; }
        public int CreatedUserId { get; set; }
        public int UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
