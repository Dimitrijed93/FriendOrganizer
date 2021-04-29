using FriendOrganizer.Model;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public interface IProgrammingLanguageRepository
        : IGenericRepository<ProgrammingLanguage>
    {
       Task<bool> IsReferencedByFriendAsync(int id);
    }
}
