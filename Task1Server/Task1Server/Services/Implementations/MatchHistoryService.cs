using Task1Server.Context;
using Task1Server.DTOs.GameDTOs;
using Task1Server.Services.Interfaces;

namespace Task1Server.Services.Implementations
{
    public class MatchHistoryService : IMatchHistoryService
    {
        private readonly AppDbContext _context;
        public MatchHistoryService(AppDbContext context)
        {
            _context = context;
        }
       
    }
}
