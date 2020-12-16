using Entities.Utils;
using System.Threading.Tasks;

namespace Business.Interfaces.Base
{
    public interface IBaseService<in T>
    {
        Task<IResponseService> Add(T entity);
        Task<IResponseService> Get(int id);
        Task<IResponseService> Update(T entity);
        Task<IResponseService> Delete(int id);
        Task<IResponseService> List();
    }
}
