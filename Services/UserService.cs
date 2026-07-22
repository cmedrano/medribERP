using Humanizer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Accounting.Data.DTOs;
using PresupuestoMVC.Helpers;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        public UserService(IUserRepository userRepository, IActivityLogRepository activityLogRepository)
        {
            _userRepository = userRepository;
            _activityLogRepository = activityLogRepository;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync(int companyId)
        {
            return await _userRepository.GetAllUsersAsync(companyId);
        }

        public async Task<UserResponseDTO> CreateUserAsync(CreateUserViewRequest userRequest)
        {
            var passwordHash = SecurityHelper.HashPassword(userRequest.Password);

            var userDto = new User
            {
                UserName = userRequest.UserName,
                UserEmail = userRequest.Email,
                UserPasswordHash = passwordHash,
                CompanyId = userRequest.CompanyId,
                Role = userRequest.Rol,
                CreateDate = DateTime.UtcNow
            };
            var result =  await _userRepository.CreateUserAsync(userDto);

            var ActivityDto = new ActivityLogRequestDto()
            {
                CompanyId = userRequest.CompanyId,
                EntityType = "User",
                Action = "CREATE",
                Description = $"Se creó un nuevo usuario {userDto.UserName}"
            };
            await _activityLogRepository.LogAsync(ActivityDto);

            return result;
        }
        public async Task<bool> ResetPassword(string email, int userId)
        {
            var randomPassword = GeneratePassword.GenerateRandomPassword(10);
            return await _userRepository.ResetPassword(email, userId, randomPassword);
        }

        public async Task<int> GetUsersCountAsync(int companyId)
        {
            var totalUsers = await _userRepository.GetUsersCountAsync(companyId);
            return totalUsers;
        }
    }
}
