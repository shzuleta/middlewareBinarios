using FBapiService.Models.Util;
using Microsoft.EntityFrameworkCore;

namespace FBapiService.DataDB
{
    public class ControlLoginCrud
    {
        public dynamic GetClienteBanco(int client, int bank)
        {
            try
            {
                using (var context = new BanticanFintechContext())
                {
                    ControlLogin registros1 = new ControlLogin();
                    var registros = context.ControlLogins.Where(p => p.IdCustomer == client && p.IdBanco == bank).ToList();
                    //registros = registros1;
                    //return registros;
                    if (registros.Count == 1)
                    {
                        return registros1 = registros.First();
                    }
                    else
                    {
                        return registros1;
                    }
                    //foreach (var registro in registros)
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
                //return 0;
            }
        }

        public dynamic GetClienteBancoIdQR (int client, int bank, string IdQR)
        {
            try
            {
                ControlLogin objLogin = new ControlLogin();
                objLogin = GetClienteBanco(client, bank);

                if (objLogin.IdCustomer != 0)
                {
                    using (var context = new BanticanFintechContext())
                    {
                        ManageQr registros1 = new ManageQr();
                        var registros = context.ManageQrs.Where(p => p.IdQr == IdQR && p.Status == "0" && p.TypeRequest.TrimEnd() == "DATAQR").ToList();
                        //registros = registros1;
                        //return registros;
                        if (registros.Count == 1)
                        {
                            registros1 = registros.First();
                            return registros1.IdQr;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                else 
                {
                    return "0";
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
                //return 0;
            }

        }

        public dynamic GetIdQR(string IdQR)
        {
            try
            {
                    using (var context = new BanticanFintechContext())
                    {
                        ManageQr registros1 = new ManageQr();
                        var registros = context.ManageQrs.Where(p => p.IdQr == IdQR && p.Status == "0" && p.TypeRequest.TrimEnd() != "CANCEL" && p.TypeRequest.TrimEnd() == "DATAQR").ToList();
                        //registros = registros1;
                        //return registros;
                        if (registros.Count == 1)
                        {
                            registros1 = registros.First();
                            return registros1.IdQr;
                        }
                        else
                        {
                            return "";
                        }
                    }

            }
            catch (Exception ex)
            {
                return ex.Message;
                //return 0;
            }

        }
    }
}
