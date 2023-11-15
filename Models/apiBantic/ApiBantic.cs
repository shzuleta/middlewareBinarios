using FBapiService.Common;
using Models.GeneraQR;
using FBapiService.Models.Util;
using Models.QrBEC;
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
using Microsoft.Extensions.Options;
using Models.QrBNB;
using Models.GeneraQRBEC;
using System.Net.NetworkInformation;
using FBapiService.Models.GeneraQRBEC;
using System.Runtime.CompilerServices;

namespace Models.apiBantic
{
    [Injectable]
    public class ApiBantic : IApiBanctic
    {
        public string Usuario { get; set; }

        public async Task<RespQRData> GetQrData(QRData dataQR)
        {
            var objQrBNB = new QrBNB.QrBNB();
            var objQrBEC = new QrBEC.QrBEC();
            var objRespuesta = new RespQRData();
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

            if (dataQR.user.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinUsuario.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinUsuario.Name.ToString();
                return objRespuesta;
            }

            QREncryptedAdmin objBNB = new QREncryptedAdmin();
            QREncryptedAdminBEC objBEC = new QREncryptedAdminBEC();

            RespQRData sQrDataBNB = new RespQRData();
            RespQRDataBEC sQrDataBEC = new RespQRDataBEC();
            switch (dataQR.codBank)
            {
                case 1:
                    //input de BNB
                    objBNB.codTransaction = dataQR.codTransaction;
                    objBNB.codBank = dataQR.codBank;

                    objBNB.trnQRData.currency = dataQR.currency;
                    objBNB.trnQRData.gloss = (dataQR.clientNote is null ? "" : dataQR.clientNote.ToString());
                    objBNB.trnQRData.amount = dataQR.amount;
                    objBNB.trnQRData.singleUse = dataQR.singleUse; //true or false
                    objBNB.trnQRData.expirationDate = dataQR.expirationDate; // "2020-07-30";
                    objBNB.trnQRData.additionalData = ""; //definir que dato va aqui
                    objBNB.trnQRData.destinationAccountId = "1"; // 1 is BOB and 2 is USD
                    break;

                case 2:
                    //input de BEC

                    objBEC.codTransaction = dataQR.codTransaction;
                    objBEC.codBank = dataQR.codBank;

                    objBEC.trnQRData.transactionId = dataQR.codTransaction;
                    objBEC.trnQRData.currency = dataQR.currency;
                    objBEC.trnQRData.description = (dataQR.clientNote is null ? "" : dataQR.clientNote.ToString());
                    objBEC.trnQRData.amount = dataQR.amount;
                    objBEC.trnQRData.singleUse = dataQR.singleUse; //true or false
                    objBEC.trnQRData.dueDate = dataQR.expirationDate; // "2020-07-30";
                    objBEC.trnQRData.modifyAmount = false; //definir si puede modificar el monto //true or false
                    objBEC.trnQRData.accountCredit = dataQR.accountCode; // cuenta en la que se recibira el cobro
                    break;

                case 3:
                    Console.WriteLine("Seleccionaste la opción 3.");
                    break;

                default:
                    objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                    objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    break;
            }
       
            try
            {
                objRespuesta.codError = "0";
                objRespuesta.descError = "";

                var IdLog = objManageQR.RegistrarManageQR("DATAQR", dataQR.codBank, dataQR.currency, dataQR.clientNote, dataQR.amount, DateTime.Parse(dataQR.expirationDate), dataQR.singleUse,
                    "", dataQR.accountCode, "", null, null, null, null, dataQR.codTransaction, dataQR.codClient, dataQR.user);

                //Validar datos con BD middleware cliente y banco
                ControlLogin objControl = new ControlLogin();
                objControl = objLogin.GetClienteBanco(dataQR.codClient, dataQR.codBank);

                string transactionIdBEC = "";

                if (objControl.ExpirationTime >= DateTime.Now)
                {
                    if (objControl.IdCustomer != 0)
                    {
                        switch (dataQR.codBank)
                        {
                            case 1:
                                //servicios de BNB
                                respTokenBNB tokenBNB = await objQrBNB.ObtenerTokenBNB();

                                sQrDataBNB = await objQrBNB.ObtenerQRData(objBNB, tokenBNB.message);

                                objRespuesta.id = sQrDataBNB.id;
                                objRespuesta.qr = sQrDataBNB.qr;
                                objRespuesta.codTransaction = dataQR.codTransaction;
                                objRespuesta.success = sQrDataBNB.success;
                                objRespuesta.message = sQrDataBNB.message;
                                objRespuesta.codError = sQrDataBNB.codError;
                                objRespuesta.descError = sQrDataBNB.descError;
                                break;

                            case 2:
                                //servicos de BEC
                                respTokenBEC tokenBEC = await objQrBEC.ObtenerTokenBEC();

                                objBEC.trnQRData.accountCredit = dataQR.accountCode;

                                sQrDataBEC = await objQrBEC.ObtenerQRData(objBEC, tokenBEC.token);

                                objRespuesta.id = sQrDataBEC.qrId;
                                objRespuesta.qr = sQrDataBEC.qrImage;
                                objRespuesta.codTransaction = dataQR.codTransaction;
                                objRespuesta.success = (sQrDataBEC.responseCode.Equals('0')) ? true : false ;
                                objRespuesta.message = sQrDataBEC.message;
                                objRespuesta.codError = sQrDataBEC.responseCode;
                                objRespuesta.descError = sQrDataBEC.descError;
                                //transactionIdBEC = sQrDataBEC.transactionId;
                                break;

                            case 3:
                                objRespuesta.codError = "";
                                objRespuesta.descError = "todavia no hay el 3";
                                break;

                            default:
                                objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                                objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                                break;
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

                objManageQR.ActualizarManageQR(objRespuesta.id, objRespuesta.success, objRespuesta.message + "--" + objRespuesta.codError + "--" + objRespuesta.descError, null, IdLog, null, "DATAQR", objRespuesta.qr);

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
            var objQrBEC = new QrBEC.QrBEC();
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

            if (value.user.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinUsuario.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinUsuario.Name.ToString();
                return objRespuesta;
            }


            QRStatus objBNB = new QRStatus();
            QRStatusBEC objBEC = new QRStatusBEC();

            RespQRStatus sQrDataBNB = new RespQRStatus();
            RespQRStatusBEC sQrDataBEC = new RespQRStatusBEC();
            switch (value.codBank)
            {
                case 1:
                    //input de BNB
                    objBNB.idQRStat.qrId = value.idQRStat.qrId;
                    objBNB.codBank = value.codBank;
                    objBNB.codClient = value.codClient;
                    objBNB.codTransaction = value.codTransaction;
                    objBNB.user = value.user;   

                   break;

                case 2:
                    //input de BEC
                    objBEC.idQRStat.qrId = value.idQRStat.qrId;
                    objBEC.codBank = value.codBank;
                    objBEC.codClient = value.codClient;
                    objBEC.codTransaction = value.codTransaction;
                    objBEC.user = value.user;
                    break;

                case 3:
                    Console.WriteLine("Seleccionaste la opción 3.");
                    break;

                default:
                    objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                    objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    break;
            }

            try                                     
            {                                    
                objRespuesta.codError = "0";                    
                objRespuesta.descError = "";                         

                var IdLog = objManageQR.RegistrarManageQR("STATUS", value.codBank, "", "", 0, DateTime.Today, false,
                    "", "-1", "", value.idQRStat.qrId, null, null, null, value.codTransaction, value.codClient, value.user);

                //Validar datos con BD middleware cliente, banco y IdQR
                var control = "";
                control = objQR.GetClienteBancoIdQR(value.codClient, value.codBank, value.idQRStat.qrId);

                if (control != "" && control != "0")
                {
                    switch (value.codBank)
                    {
                        case 1:
                            //servicios de BNB
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
                            break;

                        case 2:
                            //servicos de BEC
                            respTokenBEC tokenBEC = await objQrBEC.ObtenerTokenBEC();

                            objBEC.idQRStat.qrId = value.idQRStat.qrId;

                            sQrDataBEC = await objQrBEC.ObtenerQREstado(objBEC, tokenBEC.token);

                            //objRespuesta.idQR = (sQrDataBEC.payment.Count.Equals(0)) ? "0": sQrDataBEC.payment[0].qrId;
                            objRespuesta.idQR = objBEC.idQRStat.qrId;
                            objRespuesta.codTransaction = (sQrDataBEC.payment.Count.Equals(0)) ? "0" : sQrDataBEC.payment[0].transactionId;
                            objRespuesta.statusId = sQrDataBEC.statusQRCode;
                            objRespuesta.expirationDate = ""; // sQrDataBEC.payment.paymentDate;
                            objRespuesta.voucherId = (sQrDataBEC.payment.Count.Equals(0)) ? "0" : sQrDataBEC.payment[0].senderAccount;
                            objRespuesta.success = (sQrDataBEC.responseCode.Equals("0")) ? true : false;
                            objRespuesta.message = sQrDataBEC.message;
                            objRespuesta.codError = sQrDataBEC.responseCode;
                            objRespuesta.descError = sQrDataBEC.descError;
                            break;

                        case 3:
                            objRespuesta.codError = "";
                            objRespuesta.descError = "todavia no hay el 3";
                            break;

                        default:
                            objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                            objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                            break;
                    }
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
                   
                objManageQR.ActualizarManageQR(objRespuesta.idQR, objRespuesta.success, objRespuesta.message + "--" + objRespuesta.codError + "--" + objRespuesta.descError, null, IdLog, objRespuesta.statusId.ToString(), "STATUS", "");


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
            var objQrBEC = new QrBEC.QrBEC();
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

            if (value.user.Equals(""))
            {
                objRespuesta.codError = ErrorType.er_SinUsuario.Id.ToString();
                objRespuesta.descError = ErrorType.er_SinUsuario.Name.ToString();
                return objRespuesta;
            }

            QRCancel objBNB = new QRCancel();
            QRCancelBEC objBEC = new QRCancelBEC();

            RespQRCancel sQrDataBNB = new RespQRCancel();
            RespQRCancelBEC sQrDataBEC = new RespQRCancelBEC();
            switch (value.codBank)
            {
                case 1:
                    //input de BNB
                    objBNB.idQRCancel.qrId = value.idQRCancel.qrId;
                    objBNB.codBank = value.codBank;
                    objBNB.codClient = value.codClient;
                    objBNB.codTransaction = value.codTransaction;
                    objBNB.user = value.user;

                    break;

                case 2:
                    //input de BEC
                    objBEC.idQRCancelBEC = value.idQRCancel;
                    objBEC.codBank = value.codBank;
                    objBEC.codClient = value.codClient;
                    objBEC.codTransaction = value.codTransaction;
                    objBEC.user = value.user;
                    break;

                case 3:
                    Console.WriteLine("Seleccionaste la opción 3.");
                    break;

                default:
                    objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                    objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    break;
            }

            try
            {

                objRespuesta.id = value.codTransaction;
                objRespuesta.idQR = value.idQRCancel.qrId.ToString();
                objRespuesta.codTransaction = value.codTransaction;

                var IdLog = objManageQR.RegistrarManageQR("CANCEL", value.codBank, "", "", 0, DateTime.Today, false,
                    "", "-1", "", null, null, null, null, value.codTransaction, value.codClient, value.user);

                //Validar datos con BD middleware cliente, banco y IdQR
                var control = "";
                control = objQR.GetClienteBancoIdQR(value.codClient, value.codBank, value.idQRCancel.qrId);

                if (control != "" && control != "0") 
                {
                    switch (value.codBank)
                    {
                        case 1:
                            //servicios de BNB
                            respTokenBNB token = await objQrBNB.ObtenerTokenBNB();

                            RespQRCancel sQrData = await objQrBNB.ObtenerQRCancelar(value, token.message);
                            objRespuesta.success = sQrData.success;
                            objRespuesta.message = sQrData.message;
                            objRespuesta.codError = sQrData.codError;
                            objRespuesta.descError = sQrData.descError;
                            break;

                        case 2:
                            //servicos de BEC
                            respTokenBEC tokenBEC = await objQrBEC.ObtenerTokenBEC();

                            sQrDataBEC = await objQrBEC.ObtenerQRCancelar(objBEC, tokenBEC.token);

                            //objRespuesta.idQR = (sQrDataBEC.payment.Count.Equals(0)) ? "0": sQrDataBEC.payment[0].qrId;
                            objRespuesta.idQR = objBEC.idQRCancelBEC.qrId;
                            objRespuesta.codTransaction = objBEC.codTransaction;
                            objRespuesta.success = (sQrDataBEC.responseCode.Equals("0")) ? true : false;
                            objRespuesta.message = sQrDataBEC.message;
                            objRespuesta.codError = sQrDataBEC.responseCode;
                            objRespuesta.descError = sQrDataBEC.message;
                            break;

                        case 3:
                            objRespuesta.codError = "";
                            objRespuesta.descError = "todavia no hay el 3";
                            break;

                        default:
                            objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                            objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                            break;
                    }
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

                objManageQR.ActualizarManageQR(objRespuesta.id, objRespuesta.success, objRespuesta.message + "--" + objRespuesta.codError + "--"+ objRespuesta.descError, "", IdLog, null, "CANCEL", "");


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
            var objBankData = new BankDataCrud();
            
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

                    var bancos = objBankData.GetBankData(IdLog.IdCustomer);

                    foreach (BankDatum banks in bancos)
                    {
                        var listBank = new IDBanks();
                        listBank.idBank = banks.IdBank;
                        listBank.CodBank = banks.CodBank;
                        listBank.bank = banks.Bank;

                        objRespuesta.banks.Add(listBank);
                    }
                }
                else
                {
                        objRespuesta.codError = ErrorType.er_SinCodigos.Id.ToString();
                        objRespuesta.descError = ErrorType.er_SinCodigos.Name.ToString();
                    //objRespuesta.codError = ErrorType.er_SinClientBank.Id.ToString();
                    //objRespuesta.descError = ErrorType.er_SinClientBank.Name.ToString();
                }

            }
            catch (Exception ex)
            {
                objRespuesta.codError = ErrorType.er_Inesperado.Id.ToString();
                objRespuesta.descError = ErrorType.er_Inesperado.Name.ToString() + "---" + ex.Message;
            }
          
                return objRespuesta;           
        }

        public RespQRNotificationBEC QRNotificationBEC(QRNotificationBEC value, string usuario)
        {
            var objQrBEC = new QrBEC.QrBEC();
            var objRespuesta = new RespQRNotificationBEC();
            var objNotQR = new NotificactionCrud();
            var objQR = new ControlLoginCrud();

            if (value.QRId.Equals("0"))
            {
                objRespuesta.responseCode = 120;
                objRespuesta.message = "QRId no puede ser igual a 0";
                return objRespuesta;
            }
            if (value.QRId.Equals(""))
            {
                objRespuesta.responseCode = 121;
                objRespuesta.message = "QRId no puede ser igual a vacio";
                return objRespuesta;
            }
            if (value.QRId.Equals(null))
            {
                objRespuesta.responseCode = 122;
                objRespuesta.message = "QRId no puede ser nulo";
                return objRespuesta;
            }
            if (value.transactionId.Equals(""))
            {
                objRespuesta.responseCode = 130;
                objRespuesta.message = "La glosa esta vacio";
                return objRespuesta;
            }

            if (value.senderBankCode.Equals(0))
            {
                objRespuesta.responseCode = 140;
                objRespuesta.message = "El codigo de banco pagador no puede ser 0";
                return objRespuesta;
            }
            if (value.senderBankCode.Equals(null))
            {
                objRespuesta.responseCode = 141;
                objRespuesta.message = "El codigo de banco pagador no puede ser nulo";
                return objRespuesta;
            }

            if (value.senderName.Equals(""))
            {
                objRespuesta.responseCode = 150;
                objRespuesta.message = "El nombre del pagador no puede ser vacio";
                return objRespuesta;
            }
            if (value.senderName.Equals(null))
            {
                objRespuesta.responseCode = 151;
                objRespuesta.message = "El nombre del pagador no puede ser nulo";
                return objRespuesta;
            }

            if (value.paymentDate.Equals(null))
            {
                objRespuesta.responseCode = 160;
                objRespuesta.message = "paymentDate no puede ser nulo";
                return objRespuesta;
            }
            if (value.paymentTime.Equals(null))
            {
                objRespuesta.responseCode = 170;
                objRespuesta.message = "paymentTime no puede ser nulo";
                return objRespuesta;
            }
            if (value.currency.Equals(""))
            {
                objRespuesta.responseCode = 180;
                objRespuesta.message = "Currency no puede ser vacio";
                return objRespuesta;
            }
            if (value.currency.Equals(null))
            {
                objRespuesta.responseCode = 181;
                objRespuesta.message = "Currency no puede ser nulo";
                return objRespuesta;
            }

            if (value.amount.Equals(0))
            {
                objRespuesta.responseCode = 190;
                objRespuesta.message = "amount no puede ser cero";
                return objRespuesta;
            }

            try
            {
                objRespuesta.responseCode = 0;
                objRespuesta.message = "";

                //valida si el IdQR ya fue notificado
                var Result = objNotQR.BuscarNotificationUser(value.QRId, usuario);

                if (!Result) 
                {
                    objRespuesta.responseCode = 201;
                    objRespuesta.message = "Ya existe la notificacion del IdQR: " + value.QRId;
                }
                else
                {
                    //solo registrara los datos de la notificacion que llega del Banco
                    var IdLog = objNotQR.RegistrarNotificationQR(value.QRId, value.paymentTime, int.Parse(value.senderBankCode), value.senderName,
                        value.transactionId, value.paymentDate, value.currency + " " + value.amount.ToString(), "", "0", usuario);

                    if (IdLog is string)
                    {
                        objRespuesta.responseCode = 200;
                        objRespuesta.message = IdLog;
                    }
                    else
                    {
                        objRespuesta.responseCode = 0;
                        objRespuesta.message = "";
                    }
                }
               
            }
            catch (Exception ex)
            {
                objRespuesta.responseCode = 200;
                objRespuesta.message = ex.Message;
            }

            objQrBEC.Dispose();

            return objRespuesta;
        }
    }
}
