using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext db;
        private IDbContextTransaction transaction;
        public UnitOfWork(ApplicationDBContext db)
        {
            this.db = db;
        }
        public async Task BeginTransactionAsync()
        {
            transaction = await db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await transaction.CommitAsync();
            }
            finally
            {
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();
            db?.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {

            try
            {
                await transaction.RollbackAsync();
            }
            finally
            {
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await db.SaveChangesAsync(); 
        }
    }
}
