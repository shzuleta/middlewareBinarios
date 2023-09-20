namespace FBapiService.DataDB
{
    public class ManageTokenCrud
    {
        public ManageToken GetUserToken(string username, string password)
        {
            ManageToken registro = new ManageToken();
            try
            {
                using (var context = new BanticfintechContext())
                {
                    //ManageToken registro = new ManageToken();
                    var list = context.ManageTokens.Where(p => p.UserName == username && p.Password == password && p.Status == "0" && p.ExpirationTime >= DateTime.Now ).ToList(); 
                    //var list = context.ManageTokens.Where(p => p.UserName == username && p.Password == password && p.Status == "0").ToList();

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
                return registro;
                //return "de where" + ex.Message;
            }
        }
    }
}
