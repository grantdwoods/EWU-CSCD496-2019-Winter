namespace SecretSanta.Domain.Services
{
    public interface IRandomService
    {
        int Next();
        int Next(int maxValue);
    }
}