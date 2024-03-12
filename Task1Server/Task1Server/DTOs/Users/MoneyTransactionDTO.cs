namespace Task1Server.DTOs.Users
{
    public class MoneyTransactionDTO
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public decimal TransferAmount { get; set; }
    }
}
