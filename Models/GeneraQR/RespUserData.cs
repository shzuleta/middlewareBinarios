namespace FBapiService.Models.GeneraQR
{
    public class RespUserData
    {
        public int IdUser { get; set; }

        public string? NameUser { get; set; }

        public string? ClaveUser { get; set; }

        public string? TypeUser { get; set; }

        public int IdCustomer { get; set; }

        public string? Customer { get; set; }
        public List<IDBanks> banks { get; set; }
        public string? codError { get; set; }
        public string? descError { get; set; }

        public RespUserData()
        {
            banks = new List<IDBanks>();
        }
    }

    public class IDBanks
    {
        public int idBank { get; set; }
        public string? CodBank { get; set; }
        public string bank { get; set; }
    }
}
