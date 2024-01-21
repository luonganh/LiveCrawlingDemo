
namespace LiveCrawlingDemo
{
    public class SendHubService : ISendHubService
    {
        private readonly ILotteryService _lotteryService;
        public SendHubService(ILotteryService lotteryService)
        {
            _lotteryService = lotteryService;
        }

        public async Task AddNewLotteryResult()
        {
            await _lotteryService.AddNewLotteryResult(this);
        }
    }
}
