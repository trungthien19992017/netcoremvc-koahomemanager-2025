using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsPromotionCondition
{
    public int Id { get; set; }

    public int? PromotionId { get; set; }

    public int? ConditionType { get; set; }

    public string? ConditionTypeCode { get; set; }

    public string? ConditionSqltype { get; set; }

    public string? ConditionOperator { get; set; }

    public string? ConditionValue { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
