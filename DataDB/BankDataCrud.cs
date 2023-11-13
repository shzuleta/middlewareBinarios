using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FBapiService.DataDB
{
    public class BankDataCrud
    {

        public dynamic GetBankData(int cliente)
        {
            try
            {
                using (var context = new BanticfintechContext())
                {
                    //if (cliente > 0)
                    //{
                        BankDatum registros1 = new BankDatum();
                        var registros = context.BankData.Where(p => p.Cliente == cliente).ToList();
                        if (registros.Count >= 1)
                        {
                            //registros1 = registros.First();

                            return registros;
                        }
                        else
                        {
                            return registros1;
                        }
                    //}
                    
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
