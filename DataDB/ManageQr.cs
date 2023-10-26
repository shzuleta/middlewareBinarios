using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class ManageQr
{
    public int Id { get; set; }

    public DateTime Datesend { get; set; }

    public DateTime? Daterequest { get; set; }

    public int FkBank { get; set; }

    public int FkCustomer { get; set; }

    public string CodTransaction { get; set; } = null!;

    public string? Currency { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Expirationdate { get; set; }

    public string? Singleuse { get; set; }

    public string? Gloss { get; set; }

    public string? Status { get; set; }

    public string? Additionaldata { get; set; }

    public string? Destinationaccountid { get; set; }

    public string? Jsoninput { get; set; }

    public string? IdQr { get; set; }

    public string? VoucherId { get; set; }

    public string? Success { get; set; }

    public string? Message { get; set; }

    public string? Jsonoutput { get; set; }

    public string? TypeRequest { get; set; }

    public string? UserName { get; set; }

    public string? CodigoQr { get; set; }
}
