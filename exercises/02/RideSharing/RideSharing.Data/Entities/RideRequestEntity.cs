using RideSharing.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace RideSharing.Data.Entities;

public class RideRequestEntity
{
    [Key]
    public Guid RequestId { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropoffLocation { get; set; }
    public RideRequestStatus Status { get; set; }
}
