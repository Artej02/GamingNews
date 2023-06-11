using System.ComponentModel.DataAnnotations;
using System;

namespace GamingWeb.Custom.Models.Services
{
    public class CustomsService
    {
        public int? Id { get; set; }
        public string NameSq { get; set; }
        public string NameEn { get; set; }
        public string NameSr { get; set; }
        public string DescriptionSq { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionSr { get; set; }
        [UIHint("FileUploadService")]
        public string IconPath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
    }
}
