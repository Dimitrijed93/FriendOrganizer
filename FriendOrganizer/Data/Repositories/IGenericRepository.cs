using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task SaveAsync();

        bool HasChanges();

        void Add(T model);

        void Remove(T model);
    }
}
