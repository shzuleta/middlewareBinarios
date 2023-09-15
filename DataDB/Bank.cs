using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class Bank
{
    public int Id { get; set; }

    public string? CodBank { get; set; }

    public string? Bank1 { get; set; }

    public string? AccountId { get; set; }

    public string? AuthorizationId { get; set; }

    public string? Describe { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateUser { get; set; }

    public virtual ICollection<CustomerBank> CustomerBanks { get; set; } = new List<CustomerBank>();
}
