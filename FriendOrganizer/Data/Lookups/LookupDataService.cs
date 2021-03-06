using FriendOrganizer.Model;
using FriendOrganizerDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Lookups
{
    public class LookupDataService : ILookupDataService
    {
        private Func<FriendOrganizerDBContext> _contextCreator;

        public LookupDataService(Func<FriendOrganizerDBContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }


        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().Select(f => new LookupItem
                {
                    Id = f.Id,
                    DisplayMember = f.FirstName + " " + f.LastName
                }).ToListAsync();
            }
        }


        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking().Select(f => new LookupItem
                {
                    Id = f.Id,
                    DisplayMember = f.Name
                }).ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Meetings.AsNoTracking().Select(f => new LookupItem
                {
                    Id = f.Id,
                    DisplayMember = f.Title
                }).ToListAsync();
            }
        }

    }
}
