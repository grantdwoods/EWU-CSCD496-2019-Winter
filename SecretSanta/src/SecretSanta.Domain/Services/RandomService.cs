using System;

namespace SecretSanta.Domain.Services
{
    public class RandomService : IRandomService
    {
        private Random Random { get; set; }

        public RandomService()
        {
            Random = new Random();
        }

        public int Next()
        {
            return Random.Next();
        }

        public int Next(int maxRange)
        {
            return Random.Next(maxRange);
        }
    }
}
