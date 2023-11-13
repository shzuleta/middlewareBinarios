namespace FBapiService.Models.Util
{
    public class ErrorType: Enumeration
    {
        private ErrorType() { }
        private ErrorType(int value, string displayName) : base(value, displayName) { }

        public static readonly ErrorType er_NoExisteCuenta = new ErrorType(1000, "NO existe la cuenta para generar QR");
        public static readonly ErrorType er_FaltanDatos = new ErrorType(1002, "Faltan los siguientes campos: ");
        public static readonly ErrorType er_Internotemporal = new ErrorType(1003, "Error interno temporal: ");
        public static readonly ErrorType er_Inesperado = new ErrorType(1004, "Explicación:");

        public static readonly ErrorType er_MonedaNoExiste = new ErrorType(1005, "La moneda para generar el QR no es valida");
        public static readonly ErrorType er_MontoCero = new ErrorType(1006, "El monto es menor o igual a 0");

        public static readonly ErrorType er_NoRegistroLog = new ErrorType(3000, "No se pudo registrar el log");

        public static readonly ErrorType er_SinidQR = new ErrorType(3000, "No existe idQR igual a 0");
        public static readonly ErrorType er_SinqrId = new ErrorType(3004, "No existe qrId");
        public static readonly ErrorType er_SinCodTransaction = new ErrorType(3001, "No existe codTransaction");
        public static readonly ErrorType er_SinCodClient = new ErrorType(3002, "No esta habilitado el cliente");
        public static readonly ErrorType er_SinCodigos = new ErrorType(3003, "No esta habilitado el cliente o el banco");

        public static readonly ErrorType er_TokenInvalido = new ErrorType(3010, "Datos incorrectos o token vencido");
        public static readonly ErrorType er_SinUsuario = new ErrorType(3011, "Usuario no encontrado");
        public static readonly ErrorType er_SinClave = new ErrorType(3012, "Clave no encontrado");

        public static readonly ErrorType er_SinClientBank = new ErrorType(3012, "No tiene codigo de cliente para retornar bancos");
    }
}
