namespace FriendOrganizerDataAccess.Migrations
{
    using FriendOrganizer.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDataAccess.FriendOrganizerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDataAccess.FriendOrganizerDBContext context)
        {
            context.Friends.AddOrUpdate(
                f => f.FirstName,
                new Friend { FirstName = "Thomas", LastName ="Gruber" }

                );

            context.ProgrammingLanguages.AddOrUpdate(
                p => p.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "C++" }
                );

            context.SaveChanges();

            context.FriendPhoneNumbers.AddOrUpdate(
                 pn => pn.Number,
                 new FriendPhoneNumber { Number = "021222", FriendId = context.Friends.First().Id }
                 );
            context.Meetings.AddOrUpdate(m => m.Title,
                new Meeting
                {
                    Title = "Watching Soccer",
                    DateFrom = new DateTime(2018, 5, 26),
                    DateTo = new DateTime(2018, 5, 26),
                    Friends = new List<Friend>
                    {
                        context.Friends.Single(f => f.FirstName == "Andreas"),
                        context.Friends.Single(f => f.FirstName == "Thomas")
                    }
                });
        }
    }
}
