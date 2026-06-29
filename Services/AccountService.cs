using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<CuentaResponseDto>> GetAllAccountAsync(int companyId)
        {
            return await _accountRepository.GetAllAccountAsync(companyId);
        }
        public async Task<CuentaResponseDto> CreateAccountAsync(CreateAccountViewRequest accountRequest)
        {
            var account = new Cuenta
            {
                nombreCuenta = accountRequest.AccountName,
                SaldoInicial = accountRequest.InitialBalance,
                SaldoActual = accountRequest.InitialBalance,
                CompanyId = accountRequest.CompanyId
            };
            return await _accountRepository.CreateAccountAsync(account);
        }
        public async Task<CuentaResponseDto> CreateIncomeAsync(CreateIncomeViewRequest income)
        {
            return await _accountRepository.CreateIncomeAsync(income);
        }
        public async Task<CuentaResponseDto> CreateTransferAsync(CreateTransferViewRequest transfer)
        {
            return await _accountRepository.CreateTransferAsync(transfer);
        }
        public async Task<int> GetAccountsCountAsync(int companyId)
        {
            var totalAccounts = await _accountRepository.GetAccountsCountAsync(companyId);
            return totalAccounts;
        }

        public async Task<CuentaResponseDto> GetAccountByIdAsync(int companyId, int cuentaId)
        {
            var accounts = await _accountRepository.GetAllAccountAsync(companyId);
            return accounts.FirstOrDefault(a => a.Id == cuentaId);
        }
    }
}
