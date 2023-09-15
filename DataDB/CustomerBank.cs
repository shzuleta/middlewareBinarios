using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class CustomerBank
{
    public int Id { get; set; }

    public int FkCustomer { get; set; }

    public int FkBank { get; set; }

    public int CodBank { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateUser { get; set; }

    public virtual Bank FkBankNavigation { get; set; } = null!;

    public virtual ManCustomer FkCustomerNavigation { get; set; } = null!;
}
