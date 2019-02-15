using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services
{
    public class PairingService : IPairingService { 
        private Random Random { get; } = new Random();
        private readonly object RandomKey = new object();
        private ApplicationDbContext DbContext { get; }

        public PairingService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<bool> GeneratePairings(int groupId)
        {
            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .SingleOrDefaultAsync(x => x.Id == groupId);

            List<int> userIds = group?.GroupUsers?.Select(x => x.UserId).ToList();

            if(userIds == null|| userIds.Count < 2)
            {
                return false;
            }

            List<Pairing> pairings = await Task.Run(()=> GetPairings(userIds));

            DbContext.Pairings.AddRange(pairings);
            await DbContext.SaveChangesAsync();

            return true;
        }
        private List<Pairing> GetPairings(List<int> userIds)
        {
            int index;
            List<int> indices = new List<int>();
            List<int> randomIndices = new List<int>();

            for(int i = 0; i < userIds.Count; i++)
            {
                indices.Add(i);
            }

            lock (RandomKey)
            {
                foreach(int n in indices)
                {
                    index = Random.Next(indices.Count);

                    randomIndices.Add(indices[index]);
                    indices.Remove(index);
                }
            }

            var pairings = new List<Pairing>();

            for(int i = 0; i < userIds.Count; i++)
            {
                pairings.Add(new Pairing {
                    SantaId = userIds[randomIndices[i]],
                    RecipientId = userIds[randomIndices[i+1]]
                });
            }

            pairings.Add(new Pairing{
                SantaId = userIds[randomIndices.Last()],
                RecipientId = userIds[randomIndices.First()]
            });

            return pairings;
        }
    }
}
