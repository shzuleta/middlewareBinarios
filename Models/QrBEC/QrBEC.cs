using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Security.Policy;
using System.Runtime.CompilerServices;
using Models.logIng;
using FBapiService.Models.Util;
using FBapiService.DataDB;
using Microsoft.EntityFrameworkCore;
using Models.GeneraQR;
using Newtonsoft.Json.Linq;
using FBapiService.Models.GeneraQR;
using Models.QrBEC;
using Models.GeneraQRBEC;
using FBapiService.Models.GeneraQRBEC;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Models.QrBEC
{
    public class QrBEC : IQrBEC, ITokenBEC, IDisposable
    {
        private bool _disposed = false;

        ~QrBEC() => Dispose(false);
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<RespQRDataBEC> ObtenerQRData(QREncryptedAdminBEC idQRMotor, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string url = configuration["URL_BEC_QR"];

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                HttpContent objRespuesta;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string obSeriaJson = JsonConvert.SerializeObject(idQRMotor.trnQRData);
                objRespuesta = new StringContent(obSeriaJson, Encoding.UTF8, "application/json");
                HttpResponseMessage result = null;

                RespQRDataBEC objResobj = new RespQRDataBEC();
                objResobj.codError = "0";
                objResobj.descError = "";

                try
                {
                    result = await client.PostAsync(url, objRespuesta);

                }
                catch (Exception e)
                {
                    objResobj.codError = ErrorType.er_Inesperado.Id.ToString();
                    objResobj.descError = e.Message;
                    //var obj = e.Message;
                }

                if (!result.IsSuccessStatusCode)
                {
                    objResobj.codError = ErrorType.er_Inesperado.Id.ToString();
                    objResobj.descError = "Error en el input json";
                    //throw new Exception("No se conecto con el servidor QRDATA " + result.StatusCode.ToString());
                }
                string Resultado = await result.Content.ReadAsStringAsync();
                objResobj = JsonConvert.DeserializeObject<RespQRDataBEC>(Resultado);
                //respQREncryptedAdmin objResMidobj = JsonConvert.DeserializeObject<respQREncryptedAdmin>(Resultado);
                if (result.StatusCode.ToString().Equals("OK"))
                {
                    objResobj.codError = "0";
                }
                else
                {
                    objResobj.codError = ErrorType.er_Inesperado.Id.ToString();
                }
                objResobj.descError = result.StatusCode.ToString();

                client.Dispose();
                objRespuesta.Dispose();
                result.Dispose();

 
                return objResobj;
            }
        }

        public async Task<respTokenBEC?> ObtenerTokenBEC()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            TokenBEC inputToken = new TokenBEC();

            inputToken.username = configuration["USERNAME_BEC"];
            inputToken.password = configuration["PASSWORD_BEC"];
            string url = configuration["URL_BEC"];

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                HttpContent objRespuesta;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenQrData);

                string obSeriaJson = JsonConvert.SerializeObject(inputToken);
                objRespuesta = new StringContent(obSeriaJson, Encoding.UTF8, "application/json");
                HttpResponseMessage result = null;

                try
                {
                    result = await client.PostAsync(url, objRespuesta);

                }
                catch (Exception e)
                {
                    var obj = e.Message;
                }

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception("No se conecto con el servidor del Banco " + result.StatusCode.ToString());
                }
                string Resultado = await result.Content.ReadAsStringAsync();
                respTokenBEC objResobj = JsonConvert.DeserializeObject<respTokenBEC>(Resultado);
                //respQREncryptedAdmin objResobj = JsonConvert.DeserializeObject<respQREncryptedAdmin>(Resultado);
                client.Dispose();
                objRespuesta.Dispose();
                result.Dispose();
                return objResobj;
            }
        }

        public async Task<RespQRStatusBEC> ObtenerQREstado(QRStatusBEC value, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string url = configuration["URL_BEC_STATUSQR"];

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                //HttpContent objRespuesta;
                //HttpCompletionOption objRespuesta;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string obSeriaJson = JsonConvert.SerializeObject(value.idQRStat);
                //objRespuesta = new StringContent(obSeriaJson, Encoding.UTF8, "application/json");
                HttpResponseMessage result = null;

                RespQRStatusBEC objRes = new RespQRStatusBEC();
                objRes.responseCode = "0";
                objRes.descError = "";

                try
                {
                    result = await client.GetAsync(url + "?qrId=" + value.idQRStat.qrId);
                }
                catch (Exception e)
                {
                    objRes.responseCode = ErrorType.er_Inesperado.Id.ToString();
                    objRes.descError = e.Message;
                }

                if (!result.IsSuccessStatusCode)
                {
                    objRes.responseCode = ErrorType.er_Inesperado.Id.ToString();
                    objRes.descError = "Error en el input json";
                    //throw new Exception("No se conecto con el servidor QRDATA " + result.StatusCode.ToString());
                }
                string Resultado = await result.Content.ReadAsStringAsync();
                objRes = JsonConvert.DeserializeObject<RespQRStatusBEC>(Resultado);

                if (result.StatusCode.ToString().Equals("OK"))
                {
                    objRes.responseCode = "0";
                }
                else
                {
                    objRes.responseCode = ErrorType.er_Inesperado.Id.ToString();
                }
                //objRes.descError = (objRes.descError.Equals(null)) ? "" : objRes.descError;
                if (objRes.descError == null)
                {
                    objRes.descError = "";
                }

                client.Dispose();
                result.Dispose();

                return objRes;
            }

        }

        public async Task<RespQRCancelBEC> ObtenerQRCancelar(QRCancelBEC value, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string url = configuration["URL_BEC_CANCELQR"]; 

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                StringContent objContent;
                HttpResponseMessage response;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string obSeriaJson = JsonConvert.SerializeObject(value.idQRCancelBEC);
                objContent = new StringContent(obSeriaJson, Encoding.UTF8, "application/json");

                RespQRCancelBEC objRes = new RespQRCancelBEC();
                objRes.responseCode = "0";
                objRes.message = "";

                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                    request.Content = objContent;

                    response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string Resultado = await response.Content.ReadAsStringAsync();
                        objRes = JsonConvert.DeserializeObject<RespQRCancelBEC>(Resultado.ToString());
                        response.Dispose();
                    }
                    else
                    {
                        objRes.responseCode = response.StatusCode.ToString();
                        objRes.message = response.RequestMessage.ToString();
                        response.Dispose();
                        
                    }
                }
                catch (Exception e)
                {
                    objRes.responseCode = ErrorType.er_Inesperado.Id.ToString();
                    objRes.message = e.Message;
                    //var obj = e.Message;
                }

                client.Dispose();
                objContent.Dispose();
                //response.Dispose();

                return objRes;
            }

        }
    }

}
