using PresupuestoMVC.Areas.Accounting.Data.DTOs;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IActivityLogRepository
    {
        Task<ActivityLogResponse> LogAsync(ActivityLogRequestDto activityLogDto);
        Task<List<ActivityLogResponse>> GetRecentActivitiesAsync(int companyId);
    }
}
