using Task1Server.Entities.Common;

namespace Task1Server.Entities
{
    public class MatchHistory : BaseEntity
    {
        public string? PlayerOneMove { get; set; }

        public Guid? PlayerOneId { get; set; }
        public User? PlayerOne { get; set; }
        public string? PlayerTwoMove { get; set; }
        public Guid? PlayerTwoId { get; set; }
        public User? PlayerTwo { get; set; }
        public decimal Bet { get; set; }
        public string? Result{ get; set; }
             
        
    }
}
