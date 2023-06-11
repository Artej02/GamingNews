using System;

namespace GamingWeb.Custom.Models.User
{
    public class RoleAccess
    {
        public int? Id { get; set; }
        public int RoleId { get; set; }
        public int AccessTypeId { get; set; }
        public int ViewId { get; set; }
        public int CreatedUserId { get; set; }
        public int UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
