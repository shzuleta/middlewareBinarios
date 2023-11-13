using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class BankDatum
{
    public int Codigo { get; set; }

    public int Cliente { get; set; }

    public int IdBank { get; set; }

    public string? CodBank { get; set; }

    public string? Bank { get; set; }
}
