using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        Task Commit();
    }
}
