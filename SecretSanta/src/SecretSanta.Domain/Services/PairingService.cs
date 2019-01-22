using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class PairingService
    {
        private ApplicationDbContext Context { get; }
        public PairingService(ApplicationDbContext context)
        {
            this.Context = context;
        }

        public Pairing AddPairing(Pairing pairing)
        {
            Context.Pairings.Add(pairing);
            Context.SaveChanges();

            return pairing;
        }

        public Pairing Find(int id)
        {
            return Context.Pairings
                .Include(pairing => pairing.Recipient)
                .Include(pairing => pairing.Santa)
                .Include(pairing => pairing.Group)
                .SingleOrDefault(pairing => pairing.Id == id);
        }
    }
}