using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class DeepLinkDao : BaseDao
    {
        public async Task AddDeepLinkAsync(DeepLink deepLink)
        {
            _context.DeepLinks.Add(deepLink);
            await _context.SaveChangesAsync();
        }

        public async Task<DeepLink?> GetDeepLinkAsync(string deepLink)
        {
            return await _context.DeepLinks.FirstOrDefaultAsync(dl => dl.Link == deepLink);
        }
    }
}
