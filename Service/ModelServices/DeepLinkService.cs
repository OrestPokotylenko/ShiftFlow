using DAL;
using Model;

namespace Service.ModelServices
{
    public class DeepLinkService
    {
        private DeepLinkDao _deepLinkDao = new();

        public async Task AddDeepLinkAsync(DeepLink deepLink)
        {
            await _deepLinkDao.AddDeepLinkAsync(deepLink);
        }

        public async Task<DeepLink> GetDeepLinkAsync(string deepLink)
        {
            return await _deepLinkDao.GetDeepLinkAsync(deepLink);
        }
    }
}
