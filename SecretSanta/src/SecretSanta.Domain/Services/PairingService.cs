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
        private IRandomService Random { get; }
        private ApplicationDbContext DbContext { get; }

        public PairingService(ApplicationDbContext dbContext, IRandomService random)
        {
            Random = random ?? throw new ArgumentNullException(nameof(random));
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<List<Pairing>> GeneratePairings(int groupId)
        {
            if (groupId <= 0)
            {
                return null;
            }

            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .SingleOrDefaultAsync(x => x.Id == groupId);

            List<int> userIds = group?.GroupUsers?.Select(x => x.UserId).ToList();

            if(userIds == null|| userIds.Count < 2)
            {
                return null;
            }

            List<Pairing> pairings = await Task.Run(()=> ConfigurePairings(userIds, groupId));

            DbContext.Pairings.AddRange(pairings);
            await DbContext.SaveChangesAsync();

            return pairings;
        }
        private List<Pairing> ConfigurePairings(List<int> userIds, int groupId)
        {
            int index;
            List<int> indices = new List<int>();
            List<int> randomIndices = new List<int>();

            for(int i = 0; i < userIds.Count; i++)
            {
                indices.Add(i);
            }

            int indexRange = indices.Count;
            indexRange--;

            for(int i = 0; i < userIds.Count; i++, indexRange--)
            {
                index = Random.Next(indexRange);

                randomIndices.Add(indices[index]);
                indices.Remove(indices[index]);
            }
            
            var pairings = new List<Pairing>();

            for(int i = 0; i < userIds.Count-1; i++)
            {
                pairings.Add(new Pairing {
                    SantaId = userIds[randomIndices[i]],
                    RecipientId = userIds[randomIndices[i + 1]],
                    GroupId = groupId
                });
            }

            pairings.Add(new Pairing{
                SantaId = userIds[randomIndices.Last()],
                RecipientId = userIds[randomIndices.First()],
                GroupId = groupId
            });

            return pairings;
        }

        public async Task<List<Pairing>> GetPairingsByGroupId(int groupId)
        {
            List<Pairing> pairings = await DbContext.Pairings?.Where(x => x.GroupId == groupId).ToListAsync();
            return pairings;
        }
    }
}
