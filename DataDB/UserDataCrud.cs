namespace FBapiService.DataDB
{
    public class UserDataCrud
    {
        public dynamic GetUserData(string user, string clave)
        {
            try
            {
                using (var context = new BanticfintechContext())
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
            catch (Exception ex)
            {
                return ex.Message;
                //return 0;
            }
}
    }
}
