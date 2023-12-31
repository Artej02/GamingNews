using GamingWeb.Custom.Models.Services;
using System.Collections.Generic;
using System;

namespace GamingWeb.Custom.Models.Office
{
    public class Office
    {
        public int? Id { get; set; }
        public string NameEn { get; set; }
        public string NameSq { get; set; }
        public string NameSr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionSq { get; set; }
        public string DescriptionSr { get; set; }
        public string Code { get; set; }
        public int CreatedUserId { get; set; }
        public int UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Contact { get; set; }
        public List<Schedule.Schedule> Schedules { get; set; }
    }
}
