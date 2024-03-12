using Task1Server.Entities.Common;

namespace Task1Server.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }    
        public string Password { get; set; }
        public decimal Money { get; set; }
    }
}
