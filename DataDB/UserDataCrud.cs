using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using FBapiService.Models.GeneraQR;
using Newtonsoft.Json.Linq;


namespace FBapiService.DataDB
{
    public class UserDataCrud
    {
        public dynamic GetUserData(string user, string clave, string token)
        {
            try
            {
                string userToken = "";
                string claveToken = "";
                string dateToken = "";
                DateTime Fecha = DateTime.Now;
                using (var context = new BanticfintechContext())
                {
                    if (token != null) 
                    {
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                        var canReadToken = handler.CanReadToken(token);

                        if (canReadToken)
                        {
                            var value = handler.ReadToken(token) as JwtSecurityToken;

                            var claims = new List<Claim>();
                            foreach (var claim in value.Claims)
                            {
                                claims.Add(claim);
                            }

                            userToken = claims[0].Value;
                            claveToken = claims[1].Value;
                            dateToken = claims[2].Value;

                            if (long.TryParse(dateToken, out long unixTimestamp))
                            {
                                // Crea un objeto DateTime utilizando el valor de Unix timestamp
                                Fecha = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;

                            }
                        }
                        //return "OK";
                    }
                    if (userToken != "" && claveToken != "") 
                    {
                        UserDatum registros1 = new UserDatum();
                        var registros = context.UserData.Where(p => p.NameUser == userToken && p.ClaveUser == claveToken).ToList();
                        if (registros.Count == 1)
                        {
                            registros1 = registros.First();
                            //aqui se puede validar la fecha del token, cambiar el output de UserData 

                            return registros1;
                        }
                        else
                        {
                            return registros1;
                        }
                    }
                    else
                    {
                        UserDatum registros1 = new UserDatum();
                        var registros = context.UserData.Where(p => p.NameUser == user && p.ClaveUser == clave).ToList();
                        if (registros.Count == 1)
                        {
                            return registros1 = registros.First();
                        }
                        else
                        {
                            return registros1;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw (ex); 
                //return ex.Message;
                //return 0;
            }
        }

        public dynamic GetUserBasic(string user, string clave)
        {
            try
            {
                //string userToken = "";
                //string claveToken = "";
                //DateTime Fecha = DateTime.Now;
                using (var context = new BanticfintechContext())
                {
                    if (user != "" && clave != "")
                    {
                        UserDatum registros1 = new UserDatum();
                        var registros = context.UserData.Where(p => p.NameUser == user && p.ClaveUser == clave).ToList();
                        if (registros.Count == 1)
                        {
                            registros1 = registros.First();
                            //aqui se puede validar la fecha del token, cambiar el output de UserData 

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else 
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw (ex);
                //return ex.Message;
                //return 0;
            }

        }
    }
}
