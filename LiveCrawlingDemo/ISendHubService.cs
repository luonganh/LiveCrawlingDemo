using LiveCrawlingDemo.Models;

namespace LiveCrawlingDemo
{
    public interface ISendHubService
    {
        Task AddNewLotteryResult();

        Task SendDataToHub(LotteryViewModel lottery);
    }
}
