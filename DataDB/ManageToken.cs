using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class ManageToken
{
    public int Id { get; set; }

    public int FkCustomer { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public DateTime? ExpirationTime { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateUser { get; set; }

    public virtual ManCustomer FkCustomerNavigation { get; set; } = null!;
}
