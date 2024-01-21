
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
                await _context.Lotteries.AddAsync(new Lottery() { Date = DateTime.Now });
                await _context.SaveChangesAsync();
            }

            await Task.FromResult(0);
        }
    }
}
