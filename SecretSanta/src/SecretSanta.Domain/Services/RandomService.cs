using System;

namespace SecretSanta.Domain.Services
{
    public class RandomService : IRandomService
    {
        private Random Random { get; set; }
        private readonly object RandomKey = new object();
        public RandomService()
        {
            Random = new Random();
        }
        public int Next(int maxRange)
        {
            lock (RandomKey)
            {
                return Random.Next(maxRange);
            }
        }
    }
}
