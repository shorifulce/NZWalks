using NZWalks.api.Models.Domain;

namespace NZWalks.api.Repositories
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
        // asynchornonous korle program gulotask hoye vag hoye jai tar jonnoi Task likhi r retun hisabe task er moddhe return type likh

    }
}
