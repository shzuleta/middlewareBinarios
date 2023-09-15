using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class HistoryLog
{
    /// <summary>
    /// llave de la tabla
    /// </summary>
    public int Id { get; set; }

    public DateTime Datesend { get; set; }

    public DateTime? Daterequest { get; set; }

    public string? Level { get; set; }

    public string? Bank { get; set; }

    public string? Currency { get; set; }

    public string? Gloss { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Expirationdate { get; set; }

    public string? Singleuse { get; set; }

    public string? Additionaldata { get; set; }

    public string? Destinationaccountid { get; set; }

    public string? Jsoninput { get; set; }

    public string? IdQr { get; set; }

    public string? Success { get; set; }

    public string? Messageoutput { get; set; }

    public string? Jsonoutput { get; set; }

    public string CodeInter { get; set; } = null!;

    public string? Status { get; set; }
}
