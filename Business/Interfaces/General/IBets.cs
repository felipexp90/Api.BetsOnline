using Business.Interfaces.Base;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IBets : IBaseService<Bet>
    {
        Task<IResponseService> GetByIdGame(int idGame);
        Task<bool> BulkUpdate(IEnumerable<Bet> listBets);
    }
}
