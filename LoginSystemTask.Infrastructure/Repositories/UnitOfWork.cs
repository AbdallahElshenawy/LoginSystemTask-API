using LoginSystemTask.Application.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Interfaces;
using LoginSystemTask.Infrastructure.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        private IEmployeeRepository? _employeeRepository;
        private IUserRepository? _userRepository;
        private IRoleRepository? _roleRepository;
        private IPermissionRepository? _permissionRepository;
        private IAuditLogRepository? _auditLogRepository;        
        public IEmployeeRepository Employees
        {
            get
            {
                _employeeRepository ??= new EmployeeRepository(context, this);
                return _employeeRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                _userRepository ??= new UserRepository(context);
                return _userRepository;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                _roleRepository ??= new RoleRepository(context);
                return _roleRepository;
            }
        }

        public IPermissionRepository Permissions
        {
            get
            {
                _permissionRepository ??= new PermissionRepository(context);
                return _permissionRepository;
            }
        }

        public IAuditLogRepository AuditLogs
        {
            get
            {
                _auditLogRepository ??= new AuditLogRepository(context);
                return _auditLogRepository;
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public async Task<int> Complete()
        {
            int result;

            try
            {
                result = await context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await CommitTransactionAsync();
                }
            }
            catch (Exception)
            {
                if (_transaction != null)
                {
                    await RollbackTransactionAsync();
                }

                throw;
            }

            return result;
        }
        public void Dispose()
        {
            context.Dispose();
            _transaction?.Dispose();
        }
    }
}
    

