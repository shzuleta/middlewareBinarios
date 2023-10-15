using FBapiService.Common;
using Models.GeneraQR;
using FBapiService.Models.Util;
using Models.QrBNB;
using Microsoft.AspNetCore.Components;
using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FBapiService.DataDB;
using Microsoft.EntityFrameworkCore;
using Models.logIng;
using FBapiService.Models.GeneraQR;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;

namespace Models.apiBantic
{
    [Injectable]
    public class ApiBantic : IApiBanctic
    {
        public string Usuario { get; set; }

        public async Task<respQRData> GetQrData(QRData dataQR)
        {
            var objQrBNB = new QrBNB.QrBNB();
            var objRespuesta = new respQRData();
            var objManageQR = new ManageQRCrud();
            var objLogin = new ControlLoginCrud();
            //Validaciones 
            //var objCta = new CtaDesc(dataQR.accountCode);
            //        dataQR.businessCode = (dataQR.clientNote is null ? "" : dataQR.businessCode.ToString());

            if (dataQR.accountCode is null)
            {
                objRespuesta.codError = ErrorType.er_NoExisteCuenta.Id.ToString();
                objRespuesta.descError = ErrorType.er_NoExisteCuenta.Name.ToString();
                return objRespuesta;
            }

            dataQR.currency = (dataQR.currency is null ? "" : dataQR.currency.ToString());

            if (dataQR.currency.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_MonedaNoExiste.Id.ToString();
                objRespuesta.descError = ErrorType.er_MonedaNoExiste.Name.ToString();
                return objRespuesta;
            }
            if (!(dataQR.currency.Equals("BOB") || dataQR.currency.Equals("USD")))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Moneda " + dataQR.currency + " desconocida (debe ser BOB o USD)";
                return objRespuesta;
            }

            try
            {
                if (!(dataQR.amount > 0))
                {
                    objRespuesta.codError = ErrorType.er_MontoCero.Id.ToString();
                    objRespuesta.descError = ErrorType.er_MontoCero.Name.ToString();
                    return objRespuesta;
                }

            }
            catch (Exception ex)
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " El importe para el QR no es valido. " + ex.Message.ToString();
                return objRespuesta;
            }
    
            if (dataQR.singleUse.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese campo singleuse ( true o false)   ";
                return objRespuesta;
            }

            dataQR.expirationDate = (dataQR.expirationDate is null ? "" : dataQR.expirationDate.ToString());

            if (dataQR.expirationDate.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Fecha de vencimiento del QR no valida (formato aaaa-MM-dd)";
                return objRespuesta;
            }

            if (!(DateTime.Parse(dataQR.expirationDate) is DateTime) )
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Fecha de vencimiento del QR no valida (formato aaaa-MM-dd)";
                return objRespuesta;
            }

            if (!(DateTime.Parse(dataQR.expirationDate) >= DateTime.Now))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Fecha de vencimiento debe ser igual o mayor a hoy dia";
                return objRespuesta;
            }

            if (dataQR.codBank.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese el codigo de Banco";
                return objRespuesta;
            }

            if (dataQR.codTransaction.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinCodTransaction.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinCodTransaction.Name.ToString();
                return objRespuesta;
            }

            // Solicitar QR a Servicio de BNB 
            QREncryptedAdmin objBNB = new QREncryptedAdmin();
            //objBNB.currency = dataQR.currency;
            //objBNB.gloss = (dataQR.clientNote is null ? "" : dataQR.clientNote.ToString());
            //objBNB.amount = dataQR.amount;
            //objBNB.singleUse = dataQR.singleUse; //true or false
            //objBNB.expirationDate = dataQR.expirationDate; // "2020-07-30";
            //objBNB.additionalData = ""; //definir que dato va aqui
            //objBNB.destinationAccountId = "1"; // 1 is BOB and 2 is USD

            objBNB.codTransaction = dataQR.codTransaction;
            objBNB.codBank = dataQR.codBank;

            objBNB.trnQRData.currency = dataQR.currency;
            objBNB.trnQRData.gloss = (dataQR.clientNote is null ? "" : dataQR.clientNote.ToString());
            objBNB.trnQRData.amount = dataQR.amount;
            objBNB.trnQRData.singleUse = dataQR.singleUse; //true or false
            objBNB.trnQRData.expirationDate = dataQR.expirationDate; // "2020-07-30";
            objBNB.trnQRData.additionalData = ""; //definir que dato va aqui
            objBNB.trnQRData.destinationAccountId = "1"; // 1 is BOB and 2 is USD

            try
            {
                objRespuesta.codError = "0";
                objRespuesta.descError = "";

                var IdLog = objManageQR.RegistrarManageQR("DATAQR", dataQR.codBank, dataQR.currency, dataQR.clientNote, dataQR.amount, DateTime.Parse(dataQR.expirationDate), dataQR.singleUse,
                    "", "1", "", null, null, null, null, dataQR.codTransaction, dataQR.codClient);

                //Validar datos con BD middleware cliente y banco
                ControlLogin objControl = new ControlLogin();
                objControl = objLogin.GetClienteBanco(dataQR.codClient, dataQR.codBank);

                if (objControl.ExpirationTime >= DateTime.Now)
                {
                    if (objControl.IdCustomer != 0)
                    {
                        //servicios de BNB
                        respTokenBNB token = await objQrBNB.ObtenerTokenBNB();

                        respQRData sQrData = await objQrBNB.ObtenerQRData(objBNB, token.message);

                        objRespuesta.id = sQrData.id;
                        objRespuesta.qr = sQrData.qr;
                        objRespuesta.codTransaction = dataQR.codTransaction;
                        objRespuesta.success = sQrData.success;
                        objRespuesta.message = sQrData.message;
                        objRespuesta.codError = sQrData.codError;
                        objRespuesta.descError = sQrData.descError;

                    }
                    else
                    {
                        objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    }
                }     
                else 
                { 
                    objRespuesta.codError = ErrorType.er_TokenInvalido.ToString();
                    objRespuesta.descError = ErrorType.er_TokenInvalido.Name.ToString();
                }

                objManageQR.ActualizarManageQR(objRespuesta.id, objRespuesta.success, objRespuesta.message + "--" + objRespuesta.codError + "--" + objRespuesta.descError, null, IdLog, null, "DATAQR");

                //objRespuesta.idqr = sQrData.qrID;
                //objRespuesta.qr = sQrData.encriptedData + "|" + sQrData.bankCertSerial;
                //objRespuesta.success = sQrData.codError;
                //objRespuesta.message = ((sQrData.codError.Equals("0")) ? "Exito" : sQrData.descError);
                //objRespuesta.idqr = sQrData.qrID;
                //objRespuesta.qr = sQrData.encriptedData + "|" + sQrData.bankCertSerial;
                //objRespuesta.success = sQrData.codError;
                //objRespuesta.message = ((sQrData.codError.Equals("0")) ? "Exito" : sQrData.descError);
            }
            catch (Exception ex)
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = ErrorType.er_Inesperado.Name.ToString() + "---" + ex.Message;
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = ErrorType.er_Inesperado.Name.ToString() + "---" + ex.Message;
            }

            objQrBNB.Dispose();

            return objRespuesta;
        }
                                                                                                                    
        public async Task<RespQRStatus> GetQRStat(QRStatus value) 
        {
            var objQrBNB = new QrBNB.QrBNB();
            var objRespuesta = new RespQRStatus();
            var objManageQR = new ManageQRCrud();
            var objQR = new ControlLoginCrud();

            if (value.idQRStat.qrId.Equals(0))
            {
                objRespuesta.codError = ErrorType.er_SinidQR.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinidQR.Name.ToString();
                return objRespuesta;
            }
            if (value.codTransaction.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinCodTransaction.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinCodTransaction.Name.ToString();
                return objRespuesta;
            }

            if (value.codClient.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese el codigo de cliente";
                return objRespuesta;
            }
     
            if (value.codBank.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese el codigo de Banco";
                return objRespuesta;
            }

            if (value.codTransaction.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese el codigo interno";
                return objRespuesta;
            }
                                                                                                                                                                                                                                             
            try                                     
            {                                    
                objRespuesta.codError = "0";                    
                objRespuesta.descError = "";                         

                var IdLog = objManageQR.RegistrarManageQR("STATUS", value.codBank, "", "", 0, DateTime.Today, false,
                    "", "-1", "", null, null, null, null, value.codTransaction, value.codClient);

                //Validar datos con BD middleware cliente, banco y IdQR
                var control = "";
                control = objQR.GetClienteBancoIdQR(value.codClient, value.codBank, value.idQRStat.qrId);

                if (control != "" && control != "0")
                {
                    respTokenBNB token = await objQrBNB.ObtenerTokenBNB();

                    RespQRStatus sQrData = await objQrBNB.ObtenerQREstado(value, token.message);
                    objRespuesta.idQR = sQrData.id;
                    objRespuesta.codTransaction = value.codTransaction;
                    objRespuesta.statusId = sQrData.statusId;
                    objRespuesta.expirationDate = sQrData.expirationDate;
                    objRespuesta.voucherId = sQrData.voucherId;
                    objRespuesta.success = sQrData.success;
                    objRespuesta.message = sQrData.message;
                    objRespuesta.codError = sQrData.codError;
                    objRespuesta.descError = sQrData.descError;
                }
                else 
                {
                    if (control == "")
                    {
                        objRespuesta.codError = ErrorType.er_SinqrId.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinqrId.Name.ToString();  
                    }
                    else 
                    {
                        objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    }
                }
                   
                objManageQR.ActualizarManageQR(objRespuesta.idQR, objRespuesta.success, objRespuesta.message + "--" + objRespuesta.codError + "--" + objRespuesta.descError, null, IdLog, objRespuesta.statusId.ToString(), "STATUS");


                //objRespuesta.idqr = sQrData.qrID;
                //objRespuesta.qr = sQrData.encriptedData + "|" + sQrData.bankCertSerial;
                //objRespuesta.success = sQrData.codError;
                //objRespuesta.message = ((sQrData.codError.Equals("0")) ? "Exito" : sQrData.descError);

            }
            catch (Exception ex)
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = ErrorType.er_Inesperado.Name.ToString() + "---" + ex.Message;
            }

            objQrBNB.Dispose();
            
            return objRespuesta;
        }

        public async Task<RespQRCancel> SetQRCancel(QRCancel value)
        {
            var objQrBNB = new QrBNB.QrBNB();
            var objRespuesta = new RespQRCancel();
            var objManageQR = new ManageQRCrud();
            var objQR = new ControlLoginCrud();
            //Validaciones 

            if (value.idQRCancel.qrId.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinidQR.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinidQR.Name.ToString();
                return objRespuesta;
            }
            if (value.codTransaction.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinCodTransaction.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinCodTransaction.Name.ToString();
                return objRespuesta;
            }

            if (value.codClient.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese el codigo de cliente";
                return objRespuesta;
            }

            if (value.codBank.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese el codigo de Banco";
                return objRespuesta;
            }

            if (value.codTransaction.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = " Ingrese el codigo interno";
                return objRespuesta;
            }

            try
            {

                objRespuesta.id = value.codTransaction;
                objRespuesta.idQR = value.idQRCancel.qrId.ToString();
                objRespuesta.codTransaction = value.codTransaction;

                var IdLog = objManageQR.RegistrarManageQR("CANCEL", value.codBank, "", "", 0, DateTime.Today, false,
                    "", "-1", "", null, null, null, null, value.codTransaction, value.codClient);

                //Validar datos con BD middleware cliente, banco y IdQR
                var control = "";
                control = objQR.GetClienteBancoIdQR(value.codClient, value.codBank, value.idQRCancel.qrId);

                if (control != "" && control != "0") 
                {
                    respTokenBNB token = await objQrBNB.ObtenerTokenBNB();

                    RespQRCancel sQrData = await objQrBNB.ObtenerQRCancelar(value, token.message);
                    objRespuesta.success = sQrData.success;
                    objRespuesta.message = sQrData.message;
                    objRespuesta.codError = sQrData.codError;
                    objRespuesta.descError = sQrData.descError;
                }
                else 
                {
                    if (control == "")
                    {
                        objRespuesta.codError = ErrorType.er_SinqrId.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinqrId.Name.ToString();
                    }
                    else
                    {
                        objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    }
                }

                objManageQR.ActualizarManageQR(objRespuesta.id, objRespuesta.success, objRespuesta.message + "--" + objRespuesta.codError + "--"+ objRespuesta.descError, "", IdLog, null, "CANCEL");


                //objRespuesta.qr = sQrData.encriptedData + "|" + sQrData.bankCertSerial;
                //objRespuesta.success = sQrData.codError;
                //objRespuesta.message = ((sQrData.codError.Equals("0")) ? "Exito" : sQrData.descError);

            }
            catch (Exception ex)
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = ErrorType.er_Inesperado.Name.ToString() + "--" + ex.Message;
            }

            objQrBNB.Dispose();

            return objRespuesta;
        }

        public RespQRNotification QRNotification(QRNotification value, string usuario)
        {
            var objQrBNB = new QrBNB.QrBNB();
            var objRespuesta = new RespQRNotification();
            var objNotQR = new NotificactionCrud();
            var objQR = new ControlLoginCrud();

            if (value.QRId.Equals("0"))
            {
                objRespuesta.success = false;
                objRespuesta.message = "QRId no puede ser igual a 0";
                return objRespuesta;
            }
            if (value.QRId.Equals(""))
            {
                objRespuesta.success = false;
                objRespuesta.message = "QRId no puede ser igual a vacio";
                return objRespuesta;
            }
            if (value.QRId.Equals(null))
            {
                objRespuesta.success = false;
                objRespuesta.message = "QRId no puede ser nulo";
                return objRespuesta;
            }
            if (value.Gloss.Equals(""))
            {
                objRespuesta.success = false;
                objRespuesta.message = "La glosa esta vacio";
                return objRespuesta;
            }

            if (value.sourceBankId.Equals(0))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El codigo de banco pagador no puede ser 0";
                return objRespuesta;
            }
            if (value.sourceBankId.Equals(null))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El codigo de banco pagador no puede ser nulo";
                return objRespuesta;
            }

            if (value.originName.Equals(""))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El nombre del pagador no puede ser vacio";
                return objRespuesta;
            }
            if (value.originName.Equals(null))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El nombre del pagador no puede ser nulo";
                return objRespuesta;
            }

            if (value.VoucherId.Equals(""))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El voucherId no puede ser vacio";
                return objRespuesta;
            }
            if (value.VoucherId.Equals(null))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El voucherId no puede ser nulo";
                return objRespuesta;
            }
            if (value.VoucherId.Length.Equals(10))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El voucherId no puede ser menor a 10 caracteres";
                return objRespuesta;
            }

            if (value.TransactionDateTime.Equals(null))
            {
                objRespuesta.success = false;
                objRespuesta.message = "El TransactionDateTime no puede ser nulo";
                return objRespuesta;
            }

            try
            {
                objRespuesta.success = true;
                objRespuesta.message = "OK";

                //solo registrara los datos de la notificacion que llega del Banco
                var IdLog = objNotQR.RegistrarNotificationQR(value.QRId, value.Gloss, value.sourceBankId, value.originName,
                    value.VoucherId, value.TransactionDateTime, value.additionalData, "", "0", usuario);

                if (IdLog is string)
                {
                    objRespuesta.success = false;
                    objRespuesta.message = IdLog;
                }
                else
                {
                    objRespuesta.success = true;
                    objRespuesta.message = "OK";
                }
            }
            catch (Exception ex)
            {
                objRespuesta.success = false;
                objRespuesta.message = ex.Message;
            }

            objQrBNB.Dispose();

            return objRespuesta;
        }

        public RespGetQRNotificaction GetQRNotificaction(GetQRNotification value)
        {
            var objNotification = new GetQRNotification();
            RespGetQRNotificaction objRespuesta = new RespGetQRNotificaction();
            var objNotQR = new NotificactionCrud();
            var objLogin = new ControlLoginCrud();

            if (value.QRId.Equals(0))
            {
                objRespuesta.codError = ErrorType.er_SinidQR.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinidQR.Name.ToString();
                return objRespuesta;
            }
            if (value.codClient.Equals(0))
            {
                objRespuesta.codError = ErrorType.er_SinCodClient.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinCodClient.Name.ToString();
                return objRespuesta;
            }
            if (value.codBank.Equals(0))
            {
                objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                return objRespuesta;
            }
            if (value.codTransaction.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinCodTransaction.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinCodTransaction.Name.ToString();
                return objRespuesta;
            }

            try
            {
                objRespuesta.codError = "0";
                objRespuesta.descError = "";

                //Validar datos con BD middleware cliente y banco
                ControlLogin objControl = new ControlLogin();
                objControl = objLogin.GetClienteBanco(value.codClient, value.codBank);

                if (objControl.ExpirationTime >= DateTime.Now)
                {
                    if (objControl.IdCustomer != 0)
                    {
                        var Resp = objNotQR.BuscarNotificationQR(value.QRId, value.codClient.ToString(), value.codBank.ToString(), value.codTransaction);

                        if (Resp is string)
                        {
                            objRespuesta.codError = ErrorType.er_SinqrId.Id.ToString();
                            objRespuesta.descError = ErrorType.er_SinqrId.Name.ToString();
                        }
                        else
                        {
                            objRespuesta.QRId = Resp.IdQr;
                            objRespuesta.Gloss = Resp.Gloss;
                            objRespuesta.statusId = Resp.Status;
                            objRespuesta.TransactionDateTime = Resp.TransactionDateTime;
                        }
                    }
                    else
                    {
                        objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    }
                }
                else
                {
                    objRespuesta.codError = ErrorType.er_TokenInvalido.ToString();
                    objRespuesta.descError = ErrorType.er_TokenInvalido.Name.ToString();
                }

            }

            catch (Exception ex)
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = ErrorType.er_Inesperado.Name.ToString() + ex.Message;
            }

            return objRespuesta;
        }

        public async Task<RespUserData> GetUserData(UserData value)
        {
            var objRespuesta = new RespUserData();  
            var objUserData = new UserDataCrud();
            
            //Validaciones 
            if (value.token == null || value.token == "") 
            {
                if (value.user is null)
                {
                    objRespuesta.codError = ErrorType.er_SinUsuario.Id.ToString();
                    objRespuesta.descError = ErrorType.er_SinUsuario.Name.ToString();
                    return objRespuesta;
                }

                if (value.user.Equals(""))
                {
                    objRespuesta.codError = ErrorType.er_SinUsuario.Id.ToString();
                    objRespuesta.descError = ErrorType.er_SinUsuario.Name.ToString();
                    return objRespuesta;
                }
                if (value.password is null)
                {
                    objRespuesta.codError = ErrorType.er_SinClave.Id.ToString();
                    objRespuesta.descError = ErrorType.er_SinClave.Name.ToString();
                    return objRespuesta;
                }

                if (value.password.Equals(""))
                {
                    objRespuesta.codError = ErrorType.er_SinClave.Id.ToString();
                    objRespuesta.descError = ErrorType.er_SinClave.Name.ToString();
                    return objRespuesta;
                }
            }
            else 
            { 
                
            }

            // Solicitar QR a Servicio de BNB 
            //QREncryptedAdmin objBNB = new QREncryptedAdmin();
            //objBNB.currency = dataQR.currency;
            //objBNB.gloss = (dataQR.clientNote is null ? "" : dataQR.clientNote.ToString());
            //objBNB.amount = dataQR.amount;
            //objBNB.singleUse = dataQR.singleUse; //true or false
            //objBNB.expirationDate = dataQR.expirationDate; // "2020-07-30";
            //objBNB.additionalData = ""; //definir que dato va aqui
            //objBNB.destinationAccountId = "1"; // 1 is BOB and 2 is USD

            try
            {
                objRespuesta.codError = "0";
                objRespuesta.descError = "";

                var IdLog = objUserData.GetUserData(value.user, value.password, value.token);

                if (IdLog.IdCustomer != 0)
                {                    
                        objRespuesta.NameUser = IdLog.NameUser;
                        objRespuesta.IdUser = IdLog.IdUser;
                        objRespuesta.TypeUser = IdLog.TypeUser.Trim();
                        objRespuesta.IdCustomer = IdLog.IdCustomer;
                        objRespuesta.Customer = IdLog.Customer;
                        objRespuesta.IdBank = IdLog.IdBank;
                        objRespuesta.CodBank = IdLog.CodBank;
                        objRespuesta.Bank = IdLog.Bank;
                }
                else
                {
                        objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                }

            }
            catch (Exception ex)
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = ErrorType.er_Inesperado.Name.ToString() + "---" + ex.Message;
            }
          
                return objRespuesta;
           
        }
    }
}
