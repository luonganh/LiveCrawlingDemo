
using LiveCrawlingDemo.Models;
using Microsoft.AspNetCore.SignalR;

namespace LiveCrawlingDemo
{
    public class SendHubService : ISendHubService
    {
        private readonly ILotteryService _lotteryService;
        private readonly IHubContext<UpdateLotteryHub> _signalrHubContext;

        public SendHubService(ILotteryService lotteryService, IHubContext<UpdateLotteryHub> signalrHubContext)
        {
            _lotteryService = lotteryService;
            _signalrHubContext = signalrHubContext;
        }

        public async Task AddNewLotteryResult()
        {
            await _lotteryService.AddNewLotteryResult(this);
        }

        public async Task SendDataToHub(LotteryViewModel lottery)
        {
            await _signalrHubContext.Clients.All.SendAsync("TraditionalLottery", lottery);
        }

        public async Task UpdateLiveLotteryResult()
        {
            await _lotteryService.UpdateLiveLotteryResult(this);
        }
    }
}
