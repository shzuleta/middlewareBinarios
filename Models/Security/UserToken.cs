namespace FBapiService.Models.Security
{
    public class UserToken
    {
        public string username { get; set; }
        public string password { get; set; }
        public DateTime expiration { get; set; }
    }
}
