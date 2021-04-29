using FriendOrganizer.Model;
using FriendOrganizerDataAccess;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public class ProgrammingLanguageRepository :
        GenericRepository<ProgrammingLanguage, FriendOrganizerDBContext>,
        IProgrammingLanguageRepository
    {
        public ProgrammingLanguageRepository(FriendOrganizerDBContext context)
            : base(context)
        {

        }

        public async Task<bool> IsReferencedByFriendAsync(int id)
        {
            return await Context.Friends.AsNoTracking()
                .AnyAsync(f => f.FavoriteLanguageId == id);
        }
    }
}
