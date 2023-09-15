using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Xml.Linq;
using Models.GeneraQR;
using Models.apiBantic;
using FBapiService.Models.GeneraQR;
using FBapiService.Models.Security;
using FBapiService.Models.Util;
using Microsoft.AspNetCore.Authorization;
using FBapiService.Constant;
using FBapiService.DataDB;

namespace FBapiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MixQRController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _Configu;

        public MixQRController(IConfiguration config)
        {
            _Configu = config;
        }

        [HttpPost]
        [Route("getFBToken")]
        public RespUserToken getFBTokenn(UserToken value) 
        {
            RespUserToken rtoken = new RespUserToken();
            rtoken.codError = "0";
            rtoken.descError = "";
            String Token = "";

            if (value.username.Equals(""))
            {
                rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
                rtoken.descError = "Falta el campo username";
                return rtoken;
            }
            if (value.username == null)
            {
                rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
                rtoken.descError = "El campo username no puede ser nulo";
                return rtoken;
            }
            if (value.password.Equals(""))
            {
                rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
                rtoken.descError = "Falta el campo password";
                return rtoken;
            }
            if (value.password == null)
            {
                rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
                rtoken.descError = "El campo password no puede ser nulo";
                return rtoken;
            }
            if (value.expiration.Equals(""))
            {
                rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
                rtoken.descError = "Falta el campo expiration";
                return rtoken;
            }
            if (value.expiration.Equals("0"))
            {
                rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
                rtoken.descError = "El campo expirationMinutes no puede ser 0";
                return rtoken;
            }

            //var currentUser = UserConstant.User.FirstOrDefault(user => user.username.ToLower() == value.username.ToLower() &&
            //user.password == value.password);

            //if (currentUser == null)
            //{
            //    rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
            //    rtoken.descError = " username o password incorrectos";
            //    return rtoken;
            //}

            var jwtTokenGenerator = new FBJwtTokenGenerator(_Configu);

            // Generar el token
            try 
            {
                ManageTokenCrud objtoken = new ManageTokenCrud();
                ManageToken TOK = new ManageToken();
                TOK = objtoken.GetUserToken(value.username.ToLower(), value.password.ToLower(), value.expiration.Date);

                if (TOK.UserName != null) 
                {
                    Token = jwtTokenGenerator.GenerateToken(value.username, value.password, value.expiration);
                }
                else
                {
                    rtoken.codError = ErrorType.er_TokenInvalido.Id.ToString();
                    rtoken.descError = ErrorType.er_TokenInvalido.Name.ToString();
                }
               

            }
            catch (Exception e)
            {
                rtoken.codError = ErrorType.er_Inesperado.Id.ToString();
                rtoken.descError = e.Message;
            }

            rtoken.token = Token;

            return rtoken;
        }

        [HttpPost]
        [Authorize] 
        [Route("getQRImage")]
        public async Task<respQRData> getQRWithImage(QRData value)
        {
            var objSAAS = new ApiBantic();
            //var identity = Thread.CurrentPrincipal.Identity;
            //objSAAS.Usuario = User.Identity.Name;// identity.Name;
            var objRespuesta = new respQRData();
            objRespuesta = await objSAAS.GetQrData(value);

            if (objRespuesta.success.Equals("0"))
            {
                //var barcodedemo = new BarcodeWriter();
                //var encodignOption = new EncodingOptions() { Width = 354, Height = 354, Margin = 1, PureBarcode = false };
                //encodignOption.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                //barcodedemo.Renderer = new BitmapRenderer(); // BitmapRenderer();
                //barcodedemo.Options = encodignOption;
                //barcodedemo.Format = BarcodeFormat.QR_CODE;
                //Bitmap bitmap = barcodedemo.Write(objRespuesta.qr);

                //var context = new Acceso(_httpContextAccessor);

                //string fileSpec = context.MiMetodo();



                ////string fileSpec = System.Web.HttpContext.Current.Server.MapPath("~/Images/simple6.png");

                //var logo = new Bitmap(fileSpec);
                //var g = Graphics.FromImage(bitmap);


                //g.DrawImage(logo, new Point((bitmap.Width - logo.Width) / 2, (bitmap.Height - logo.Height) / 2));

                ///*RectangleF rectf = new RectangleF(45, 320, 0, 50);
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //g.DrawString("Venc:", new Font("Arial", 9), Brushes.Black, rectf);
                //g.Flush(); */


                //using (var ms = new MemoryStream())
                //{
                //    using (var bitmap1 = new Bitmap(bitmap))
                //    {
                //        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //        var SigBase64 = Convert.ToBase64String(ms.GetBuffer());
                //        objRespuesta.qr = SigBase64;
                //    }
                //}


            }

            return objRespuesta;
        }

        [HttpPost]
        [Authorize]
        [Route("getQRStatus")]
        public async Task<RespQRStatus> getQRStatus(QRStatus value)
        {
            var objSAAS = new ApiBantic();
            //var identity = Thread.CurrentPrincipal.Identity;
            //objSAAS.Usuario = identity.Name;
            var objRespuesta = new RespQRStatus();
            objRespuesta = await objSAAS.GetQRStat(value);

            if (objRespuesta.success.Equals("0"))
            {
                //var barcodedemo = new BarcodeWriter();
                //var encodignOption = new EncodingOptions() { Width = 354, Height = 354, Margin = 1, PureBarcode = false };
                //encodignOption.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                //barcodedemo.Renderer = new BitmapRenderer(); // BitmapRenderer();
                //barcodedemo.Options = encodignOption;
                //barcodedemo.Format = BarcodeFormat.QR_CODE;
                //Bitmap bitmap = barcodedemo.Write(objRespuesta.qr);

                //var context = new Acceso(_httpContextAccessor);

                //string fileSpec = context.MiMetodo();



                ////string fileSpec = System.Web.HttpContext.Current.Server.MapPath("~/Images/simple6.png");

                //var logo = new Bitmap(fileSpec);
                //var g = Graphics.FromImage(bitmap);


                //g.DrawImage(logo, new Point((bitmap.Width - logo.Width) / 2, (bitmap.Height - logo.Height) / 2));

                ///*RectangleF rectf = new RectangleF(45, 320, 0, 50);
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //g.DrawString("Venc:", new Font("Arial", 9), Brushes.Black, rectf);
                //g.Flush(); */


                //using (var ms = new MemoryStream())
                //{
                //    using (var bitmap1 = new Bitmap(bitmap))
                //    {
                //        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //        var SigBase64 = Convert.ToBase64String(ms.GetBuffer());
                //        objRespuesta.qr = SigBase64;
                //    }
                //}


            }

            return objRespuesta;
        }

        [HttpPost]
        [Authorize]
        [Route("setQRCancel")]
        public async Task<RespQRCancel> setQRCancel(QRCancel value)
        {
            var objSAAS = new ApiBantic();
            //var identity = Thread.CurrentPrincipal.Identity;
            //objSAAS.Usuario = identity.Name;
            var objRespuesta = new RespQRCancel();
            objRespuesta = await objSAAS.SetQRCancel(value);
                                                                                  
            return objRespuesta;
        }

        [HttpPost]
        [Authorize]
        [Route("ReceiveNotificationBNB")]
        public async Task<RespQRNotification> ReceiveNotification(QRNotification value) 
        {
            var objSAAS = new ApiBantic();
            //var identity = Thread.CurrentPrincipal.Identity;
            //objSAAS.Usuario = identity.Name;
            var objRespuesta = new RespQRNotification();
            objRespuesta = await objSAAS.QRNotification(value, "BNB");

            return objRespuesta;
        }

        [HttpPost]
        [Authorize]
        [Route("GetNotification")]
        public async Task<RespGetQRNotificaction> GetNotification(GetQRNotification value)
        {
            var objSAAS = new ApiBantic();
            //var identity = Thread.CurrentPrincipal.Identity;
            //objSAAS.Usuario = identity.Name;
            var objRespuesta = new RespGetQRNotificaction();
            //objRespuesta = await objSAAS.GetQRNotificaction(value);

            return objRespuesta;
        }
    }                                             
}
