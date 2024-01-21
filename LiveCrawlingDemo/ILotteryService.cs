namespace LiveCrawlingDemo
{
    public interface ILotteryService
    {
        Task AddNewLotteryResult(ISendHubService sendHubService);
    }
}
