using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetValidation
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Type { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? Pattern { get; set; }

    public string? Store { get; set; }

    public string? Message { get; set; }

    public int? IsActive { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public int? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public int? DatasourceId { get; set; }
}
