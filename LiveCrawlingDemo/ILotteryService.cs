using LiveCrawlingDemo.Models;

namespace LiveCrawlingDemo
{
    public interface ILotteryService
    {        
        Task AddNewLotteryResult(ISendHubService sendHubService);

        Task<LotteryViewModel> GetLatestLotteryResultAsync();

        LotteryViewModel GetLatestLotteryResult();

        Task UpdateLiveLotteryResult(ISendHubService sendHubService);
    }
}
