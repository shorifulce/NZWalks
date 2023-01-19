using NZWalks.api.Models.Domain;

namespace NZWalks.api.Repositories
{
    public interface ITokenHandlerRepository
    {
        Task<string> CreateTokenAsync(User user);
    }
}
