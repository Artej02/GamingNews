using System;
using System.ComponentModel.DataAnnotations;

namespace GamingWeb.Custom.Models.Services
{
    public class Service
    {
        public int? Id { get; set; }
        public string NameSq { get; set; }
        public string NameEn { get; set; }
        public string NameSr { get; set; }
        [UIHint("FileUploadService")]
        public string IconPath { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public int? DirectoryId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectorId { get; set; }
        public int? OfficeId { get; set; }
    }
}
