using System;
using System.Collections.Generic;

namespace FBapiService.DataDB;

public partial class ControlLogin
{
    public int IdCustomer { get; set; }

    public string? NombCustomer { get; set; }

    public string? TipoCustomer { get; set; }

    public string? EstadoCustomer { get; set; }

    public DateTime? CreacionCustomer { get; set; }

    public string? UsuarioCustomer { get; set; }

    public int IdBanco { get; set; }

    public string? CodBank { get; set; }

    public string? Bank { get; set; }

    public string? CuentaBanco { get; set; }

    public string? AutorizacionBanco { get; set; }

    public string? DescripcionBanco { get; set; }

    public string? TipoBanco { get; set; }

    public string? EstadoBanco { get; set; }

    public DateTime? CreacionBanco { get; set; }

    public int IdRelacion { get; set; }
}
