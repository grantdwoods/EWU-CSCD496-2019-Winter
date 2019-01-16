using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class GiftService
    {
        private ApplicationDbContext Context { get; set; }
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
            return Context.Gifts.Include(gift => gift.User)
                .SingleOrDefault(gift => gift.Id == id);
        }
        public Gift RemoveGift(Gift gift)
        {
            Context.Gifts.Remove(gift);
            Context.SaveChanges();

            return gift;
        }

        public List<Gift> FetchAllUserGifts(int id)
        {
            var user = Context.Users.Include(u => u.Gifts)
                .SingleOrDefault(u => u.Id == id);

            return user.Gifts.ToList();
        }
    }
}
