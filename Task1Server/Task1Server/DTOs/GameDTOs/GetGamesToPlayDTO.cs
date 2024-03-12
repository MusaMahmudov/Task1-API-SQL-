using Task1Server.Entities;

namespace Task1Server.DTOs.GameDTOs
{
    public class GetGamesToPlayDTO
    {
        public string? PlayerOne { get; set; }
       
        public decimal Bet { get; set; }

    }
}
