using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsPromotion
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Code { get; set; }

    public int? PromotionType { get; set; }

    public string? PromotionTypeCode { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsAutomationApply { get; set; }

    public bool? IsApplyOncePerUser { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public string? PromotionContent { get; set; }
}
