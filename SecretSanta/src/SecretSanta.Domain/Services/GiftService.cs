using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class GiftService
    {
        public ApplicationDbContext Context { get; }
        public GiftService(ApplicationDbContext context)
        {
            this.Context = context;
        }

        public Gift UpsertUserGift(Gift gift, User user)
        {
            ICollection<Gift> gifts = Context.Users.Find(user).Gifts;
            return null;
        }
    }
}
