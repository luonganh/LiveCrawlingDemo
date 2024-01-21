using LiveCrawlingDemo.Models;
using Microsoft.AspNetCore.SignalR;

namespace LiveCrawlingDemo
{
    public class UpdateLotteryHub : Hub
    {
        public async Task TraditionalLottery(LotteryViewModel message)
        {
            await Clients.All.SendAsync("TraditionalLottery", message);
        }
    }
}
