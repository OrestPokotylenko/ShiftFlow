using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class DeepLinkDao
    {
        private ShiftFlowContext _context = new();

        public async Task AddDeepLinkAsync(DeepLink deepLink)
        {
            await _context.DeepLinks.AddAsync(deepLink);
            await _context.SaveChangesAsync();
        }

        public async Task<DeepLink> GetDeepLinkAsync(string deepLink)
        {
            return await _context.DeepLinks.FirstOrDefaultAsync(dl => dl.Link == deepLink);
        }
    }
}
