namespace FBapiService.DataDB
{
    public class ManageTokenCrud
    {
        public dynamic GetUserToken(string username, string password, System.DateTime expirationMinutes)
        {
            try
            {
                using (var context = new BanticanFintechContext())
                {
                    ManageToken registro = new ManageToken();
                    var list = context.ManageTokens.Where(p => p.UserName == username && p.Password == password && p.Status == "0" && p.ExpirationTime >= expirationMinutes ).ToList(); 

                    if (list.Count == 1)
                    {
                        return registro = list.First();
                    }
                    else
                    {
                        return registro;
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
