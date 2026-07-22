using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Accounting.Data.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRespository;

        public ActivityLogService(IActivityLogRepository activityLogRepository)
        {
            _activityLogRespository = activityLogRepository;
        }

        public async Task<List<ActivityLogResponse>> GetRecentActivitiesAsync(int companyId)
        {
            return await _activityLogRespository.GetRecentActivitiesAsync(companyId);
        }

        public async Task<ActivityLogResponse> LogAsync(ActivityLogRequestDto activityLogDto)
        {
            return await _activityLogRespository.LogAsync(activityLogDto);
        }
    }
}
