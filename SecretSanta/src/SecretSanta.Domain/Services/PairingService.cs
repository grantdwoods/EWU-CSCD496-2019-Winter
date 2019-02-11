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
    class PairingService : IPairingService
    {
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

            await DbContext.Pairings.AddRangeAsync(pairings);
            await DbContext.SaveChangesAsync();

            return true;
        }
        private List<Pairing> GetPairings(List<int> userIds)
        {
            var pairings = new List<Pairing>();

            for(int i = 0; i < userIds.Count; i++)
            {
                pairings.Add(new Pairing {
                    SantaId = userIds[i],
                    RecipientId = userIds[i+1] });
            }

            pairings.Add(new Pairing
            {
                SantaId = userIds.Last(),
                RecipientId = userIds.First()
            });

            return pairings;
        }
    }
}
