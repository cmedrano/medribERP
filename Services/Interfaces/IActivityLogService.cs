using PresupuestoMVC.Areas.Accounting.Data.DTOs;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<ActivityLogResponse> LogAsync(ActivityLogRequestDto activityLogDto);
        Task<List<ActivityLogResponse>> GetRecentActivitiesAsync(int companyId);
    }
}
