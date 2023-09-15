using Models.GeneraQR;
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
using FBapiService.Models.GeneraQR;
using Newtonsoft.Json.Linq;

namespace Models.QrBNB
{
    public class QrBNB : IQrBNB, ITokenBNB, IDisposable
    {
        private bool _disposed = false;

        ~QrBNB() => Dispose(false);
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

        public async Task<respQRData> ObtenerQRData(QREncryptedAdmin idQRMotor, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string url = configuration["URL_BNB_QR"];

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

                respQRData objResobj = new respQRData();
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
                objResobj = JsonConvert.DeserializeObject<respQRData>(Resultado);
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

        public async Task<respTokenBNB?> ObtenerTokenBNB()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            TokenBNB inputToken = new TokenBNB();

            inputToken.accountID = configuration["ACCOUNTID_BNB"];
            inputToken.authorizationID = configuration["AUTHORIZATIONID_BNB"];
            string url = configuration["URL_BNB"];

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
                respTokenBNB objResobj = JsonConvert.DeserializeObject<respTokenBNB>(Resultado);
                //respQREncryptedAdmin objResobj = JsonConvert.DeserializeObject<respQREncryptedAdmin>(Resultado);
                client.Dispose();
                objRespuesta.Dispose();
                result.Dispose();
                return objResobj;
            }
        }

        public async Task<RespQRStatus> ObtenerQREstado(QRStatus value, string token) 
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string url = configuration["URL_BNB_IDQR"];

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                HttpContent objRespuesta;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string obSeriaJson = JsonConvert.SerializeObject(value.idQRStat);
                objRespuesta = new StringContent(obSeriaJson, Encoding.UTF8, "application/json");
                HttpResponseMessage result = null;

                RespQRStatus objRes = new RespQRStatus();
                objRes.codError = "0";
                objRes.descError = "";

                try
                {
                    result = await client.PostAsync(url, objRespuesta);

                }
                catch (Exception e)
                {
                    objRes.codError = ErrorType.er_Inesperado.Id.ToString();
                    objRes.descError = e.Message;
                    //var obj = e.Message;
                }

                if (!result.IsSuccessStatusCode)
                {
                    objRes.codError = ErrorType.er_Inesperado.Id.ToString();
                    objRes.descError = "Error en el input json";
                    //throw new Exception("No se conecto con el servidor QRDATA " + result.StatusCode.ToString());
                }
                string Resultado = await result.Content.ReadAsStringAsync();
                objRes = JsonConvert.DeserializeObject<RespQRStatus>(Resultado);
                //respQREncryptedAdmin objResMidobj = JsonConvert.DeserializeObject<respQREncryptedAdmin>(Resultado);
                if (result.StatusCode.ToString().Equals("OK"))
                {
                    objRes.codError = "0";
                }
                else
                {
                    objRes.codError = ErrorType.er_Inesperado.Id.ToString();
                }
                objRes.descError = result.StatusCode.ToString();

                client.Dispose();
                objRespuesta.Dispose();
                result.Dispose();


                return objRes;
            }

        }

        public async Task<RespQRCancel> ObtenerQRCancelar(QRCancel value, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string url = configuration["URL_BNB_CANCELQR"];

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                HttpContent objRespuesta;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();                                                                                                                                                                                                                                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);                    

                string obSeriaJson = JsonConvert.SerializeObject(value.idQRCancel);
                objRespuesta = new StringContent(obSeriaJson, Encoding.UTF8, "application/json");
                HttpResponseMessage result = null;
                                                         
                RespQRCancel objRes = new RespQRCancel();
                objRes.codError = "0";
                objRes.descError = "";

                try
                {
                    result = await client.PostAsync(url, objRespuesta);

                }
                catch (Exception e)
                {
                    objRes.codError = ErrorType.er_Inesperado.Id.ToString();
                    objRes.descError = e.Message;
                    //var obj = e.Message;
                }

                if (!result.IsSuccessStatusCode)
                {
                    objRes.codError = ErrorType.er_Inesperado.Id.ToString();
                    objRes.descError = "Error en el input json";
                    //throw new Exception("No se conecto con el servidor QRDATA " + result.StatusCode.ToString());
                }
                string Resultado = await result.Content.ReadAsStringAsync();
                objRes = JsonConvert.DeserializeObject<RespQRCancel>(Resultado);
                if(result.StatusCode.ToString().Equals("OK"))
                {
                    objRes.codError = "0";
                }
                else 
                {
                    objRes.codError = ErrorType.er_Inesperado.Id.ToString();
                }
                objRes.descError = result.StatusCode.ToString();

                client.Dispose();
                objRespuesta.Dispose();
                result.Dispose();

                return objRes;
            }

        }
    }

}
