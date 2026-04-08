using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<CuentaResponseDto>> GetAllAccountAsync(int companyId);
        Task<CuentaResponseDto> CreateAccountAsync(CreateAccountViewRequest accountRequest);
        Task<CuentaResponseDto> CreateIncomeAsync(CreateIncomeViewRequest income);
        Task<CuentaResponseDto> CreateTransferAsync(CreateTransferViewRequest transfer);
        Task<int> GetAccountsCountAsync(int companyId);
    }
}
