﻿using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsServicePriceByRoom
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int ServiceId { get; set; }

    public int? Quantity { get; set; }

    public decimal? ServicePrice { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
