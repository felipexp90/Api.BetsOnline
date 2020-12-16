using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }
        public UnitOfWork(DbContext context) => Context = context;
        public Task Commit() => Context.SaveChangesAsync();
        public void Dispose() => Context.Dispose();
    }
}
