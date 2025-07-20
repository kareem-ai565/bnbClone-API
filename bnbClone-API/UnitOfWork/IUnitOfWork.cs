using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPropertyRepo PropertyRepo { get; }
        IPropertyImageRepo PropertyImageRepo { get; }
        ICancellationPolicyRepo CancellationPolicies { get; }


        Task<int> CompleteAsync();
    }
}
