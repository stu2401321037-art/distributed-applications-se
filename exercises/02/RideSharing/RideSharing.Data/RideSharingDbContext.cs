using Microsoft.EntityFrameworkCore;
using RideSharing.Data.Entities;

namespace RideSharing.Data;

public class RideSharingDbContext : DbContext, IDbContext
{
    public RideSharingDbContext(DbContextOptions<RideSharingDbContext> options)
        : base(options)
    {
    }

    public DbSet<RideRequestEntity> RideRequests => Set<RideRequestEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
