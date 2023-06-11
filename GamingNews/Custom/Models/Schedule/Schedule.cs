using System;
using System.Data.Common;

namespace GamingWeb.Custom.Models.Schedule
{
    public class Schedule
    {
        public int? Id { get; set; }
        public int DayFrom { get; set; }
        public int DayTo { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public bool IsClosed { get; set; }
        public int? DirectoryId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectorId { get; set; }
        public int? OfficeId { get; set; }
    }
}
