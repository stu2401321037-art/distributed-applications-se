using RideSharing.Data.Entities;

namespace RideSharing.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<RideRequestEntity> RideRequests { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
