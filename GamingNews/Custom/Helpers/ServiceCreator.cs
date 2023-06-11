using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Models;
using GamingWeb.Custom.Models.Department;
using GamingWeb.Custom.Models.Office;
using GamingWeb.Custom.Models.Schedule;
using GamingWeb.Custom.Models.Sector;
using GamingWeb.Custom.Models.Services;
using static Azure.Core.HttpHeader;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace GamingWeb.Custom.Helpers
{
    public class OrgServiceCreator
    {

        public async void UpdateOrgService(OrgService orgService, int? dirId, int? depId, int? secId, int? offId)
        {
            var updateResult = await new Query().Execute("CreateUpdateDeleteOrgService @CRUDOperation,@Id,@ServiceId,@DirectoryId,@DepartmentId,@SectorId,@OfficeId", new
            {
                @CRUDOperation = CRUDOperation.Update,
                @Id = orgService.Id,
                @ServiceId = orgService.ServiceId,
                @DirectoryId = dirId,
                @DepartmentId = depId,
                @SectorId = secId,
                @OfficeId = offId
            });

        }

        public async void DeleteOrgService(int? Id)
        {
            var deleteResult = await new Query().Execute("CreateUpdateDeleteService @CRUDOperation,@Id", new
            {
                @CRUDOperation = CRUDOperation.Delete,
                @Id = Id
            });
        }
    }
}
