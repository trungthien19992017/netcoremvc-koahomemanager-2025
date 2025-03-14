using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsCustomer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Cccd { get; set; }

    public string? Description { get; set; }

    public string? Mxh { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public virtual ICollection<HsBooking> HsBookings { get; set; } = new List<HsBooking>();

    public virtual ICollection<HsReview> HsReviews { get; set; } = new List<HsReview>();
}
