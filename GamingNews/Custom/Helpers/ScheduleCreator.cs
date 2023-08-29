//using GamingWeb.Custom.DatabaseHelpers;
//using GamingWeb.Custom.Models;
//using GamingWeb.Custom.Models.Department;
//using GamingWeb.Custom.Models.Office;
//using GamingWeb.Custom.Models.Schedule;
//using GamingWeb.Custom.Models.Sector;

//namespace GamingWeb.Custom.Helpers
//{
//    public class ScheduleCreator
//    {
//        public async void CreateSchedule(Schedule schedule, int? dirId, int? depId, int? secId, int? offId)
//        {
//            var createResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteSchedule @CRUDOperation,@Id,@DayFrom,@DayTo,@TimeFrom,@TimeTo,@IsClosed,@DirectoryId,@DepartmentId,@SectorId,@OfficeId", new
//            {
//                @CRUDOperation = CRUDOperation.Create,
//                @Id = schedule.Id,
//                @DayFrom = schedule.DayFrom,
//                @DayTo = schedule.DayTo,
//                @TimeFrom = schedule.TimeFrom,
//                @TimeTo = schedule.TimeTo,
//                @IsClosed = schedule.IsClosed,
//                @DirectoryId = dirId,
//                @DepartmentId = depId,
//                @SectorId = secId,
//                @OfficeId = offId
//            });

//        }

//        public async void UpdateSchedule(Schedule schedule, int? dirId, int? depId, int? secId, int? offId)
//        {
//            var updateResult = await new Query().Execute("CreateUpdateDeleteSchedule @CRUDOperation,@Id,@DayFrom,@DayTo,@TimeFrom,@TimeTo,@IsClosed,@DirectoryId,@DepartmentId,@SectorId,@OfficeId", new
//            {
//                @CRUDOperation = CRUDOperation.Update,
//                @Id = schedule.Id,
//                @DayFrom = schedule.DayFrom,
//                @DayTo = schedule.DayTo,
//                @TimeFrom = schedule.TimeFrom,
//                @TimeTo = schedule.TimeTo,
//                @IsClosed = schedule.IsClosed,
//                @DirectoryId = dirId,
//                @DepartmentId = depId,
//                @SectorId = secId,
//                @OfficeId = offId
//            });

//        }

//        public async void DeleteSchedule(int? Id)
//        {
//            var deleteResult = await new Query().Execute("CreateUpdateDeleteSchedule @CRUDOperation,@Id", new
//            {
//                @CRUDOperation = CRUDOperation.Delete,
//                @Id = Id
//            });
//        }
//    }
//}
