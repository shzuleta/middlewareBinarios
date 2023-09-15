using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class Notification
{
    public int Id { get; set; }

    public string IdQr { get; set; } = null!;

    public string? Gloss { get; set; }

    public string? SouceCodBank { get; set; }

    public string? OriginName { get; set; }

    public string? VoucherId { get; set; }

    public DateTime? TransactionDateTime { get; set; }

    public string? Additionaldata { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateUser { get; set; }
}
