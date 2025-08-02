using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsPromotionReward
{
    public int Id { get; set; }

    public int? PromotionId { get; set; }

    public int? RewardType { get; set; }

    public string? RewardTypeCode { get; set; }

    public string? RewardValue { get; set; }

    public int? MaxDiscount { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
