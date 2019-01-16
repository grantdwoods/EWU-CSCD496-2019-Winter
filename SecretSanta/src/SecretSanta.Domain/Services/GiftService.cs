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

        public Gift AddGift(Gift gift)
        {
            Context.Gifts.Add(gift);
            Context.SaveChanges();
            return gift;
        }
        public Gift UpdateGift(Gift gift)
        {
            Context.Gifts.Update(gift);
            Context.SaveChanges();
            return gift;
        }

        public Gift Find(int id)
        {
            return Context.Gifts.Find(id);
        }
        public Gift RemoveGift(Gift gift)
        {
            Context.Gifts.Remove(gift);
            Context.SaveChanges();
            return gift;
        }
    }
}
