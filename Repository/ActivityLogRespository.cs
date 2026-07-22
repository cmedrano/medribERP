using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Accounting.Data.DTOs;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class ActivityLogRespository : IActivityLogRepository
    {
        private readonly AppDbContext _context;

        public ActivityLogRespository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityLogResponse>> GetRecentActivitiesAsync(int companyId)
        {
            return await _context.ActivityLog
                .Where(a => a.CompanyId == companyId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new ActivityLogResponse
                {
                    CompanyId = a.CompanyId,
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                })
                .ToListAsync();
        }

        public async Task<ActivityLogResponse> LogAsync(ActivityLogRequestDto activityLogDto)
        {
            try
            {
                var activityDto = new ActivityLog
                {
                    CompanyId = activityLogDto.CompanyId,
                    UserId = activityLogDto.UserId,
                    EntityType = activityLogDto.EntityType,
                    EntityId = activityLogDto.EntityId,
                    Action = activityLogDto.Action,
                    Description = activityLogDto.Description
                    //OldValues = JsonSerializer.Serialize(cliente)
                };

                _context.ActivityLog.Add(activityDto);

                await _context.SaveChangesAsync();

                return new ActivityLogResponse
                {
                    CompanyId = activityDto.CompanyId,
                    Action = activityDto.Action,
                    Description = activityDto.Action
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
