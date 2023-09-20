using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class UserDatum
{
    public int IdUser { get; set; }

    public string? NameUser { get; set; }

    public string? ClaveUser { get; set; }

    public string? TypeUser { get; set; }

    public int IdCustomer { get; set; }

    public string? Customer { get; set; }

    public int IdBank { get; set; }

    public string? CodBank { get; set; }

    public string? Bank { get; set; }
}
