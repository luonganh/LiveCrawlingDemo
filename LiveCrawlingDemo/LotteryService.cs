
using LiveCrawlingDemo.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiveCrawlingDemo
{
    public class LotteryService : ILotteryService
    {
        private readonly AppDbContext _context;
        

        public LotteryService(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task AddNewLotteryResult(ISendHubService sendHubService)
        {
            if (_context.Lotteries?.Any(x => x.Date.Date == DateTime.Now.Date) == false) 
            {
                var newResult = new Lottery() { Date = DateTime.Now };
                await _context.Lotteries.AddAsync(newResult);
                int addSuccessfullly = await _context.SaveChangesAsync();
                if (addSuccessfullly > 0)
                {
                    var model = new LotteryViewModel
                    {
                        Id = newResult.Id,                                           
                        Date = newResult.Date.ToString("dd-MM-yyyy")                        
                    };
                    await sendHubService.SendDataToHub(model);
                }                
            }
            await Task.FromResult(0);
        }
       

        public async Task<LotteryViewModel> GetLatestLotteryResultAsync()
        {
            return await (from l in _context.Lotteries
                          orderby l.Date descending
                          select new LotteryViewModel
                          {
                              Id = l.Id,
                              Date = l.Date.ToString("dd-MM-yyyy"),
                              SpecialPrize = l.SpecialPrize,
                              Prize1st = l.Prize1st
                          })
                        .FirstOrDefaultAsync() ?? new LotteryViewModel();
        }

        public LotteryViewModel GetLatestLotteryResult()
        {
            return (from l in _context.Lotteries
                          orderby l.Date descending
                          select new LotteryViewModel
                          {
                              Id = l.Id,
                              Date = l.Date.ToString("dd-MM-yyyy"),
                              SpecialPrize = l.SpecialPrize,
                              Prize1st = l.Prize1st
                          })
                        .FirstOrDefault() ?? new LotteryViewModel();
        }
    }
}
