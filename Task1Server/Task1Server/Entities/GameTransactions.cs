using Task1Server.Entities.Common;

namespace Task1Server.Entities
{
    public class GameTransactions : BaseEntity
    {
        public Guid SenderId { get; set; }
        public User Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public User Receiver { get; set; }
        public decimal Money { get; set; }

    }
}
