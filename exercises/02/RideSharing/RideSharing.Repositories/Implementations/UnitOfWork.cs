using RideSharing.Data;
using RideSharing.Data.Entities;
using RideSharing.Repositories.Interfaces;

namespace RideSharing.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _context;
    private IRepository<RideRequestEntity>? _rideRequests;

    public UnitOfWork(IDbContext context)
    {
        _context = context;
    }

    public IRepository<RideRequestEntity> RideRequests
        => _rideRequests ??= new Repository<RideRequestEntity>(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        if (_context is IDisposable disposable)
            disposable.Dispose();
    }
}
