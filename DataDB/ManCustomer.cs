using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class ManCustomer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Describe { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateUser { get; set; }

    public virtual ICollection<CustomerBank> CustomerBanks { get; set; } = new List<CustomerBank>();

    public virtual ICollection<ManageToken> ManageTokens { get; set; } = new List<ManageToken>();
}
