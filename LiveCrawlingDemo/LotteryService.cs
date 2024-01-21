
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using LiveCrawlingDemo.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task UpdateLiveLotteryResult(ISendHubService sendHubService)
        {
            if (_context.Lotteries?.Any(x => x.Date.Date == DateTime.Now.Date) == true)
            {
                var result = _context.Lotteries.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date);
                HtmlDocument document = new HtmlDocument();
                var resultModel = new LotteryViewModel
                {
                    Id = result.Id,
                    SpecialPrize = result.SpecialPrize,
                    Prize1st = result.Prize1st,
                    Date = result.Date.ToString("dd-MM-yyyy")
                };
                var domSelector = new LotteryDOMViewModel();
                var prizeSelectors = domSelector.GetType().GetProperties();
                var hasUpdated = false;
                foreach (var prizeModel in resultModel.GetType().GetProperties())
                {
                    //var val = (string)GetType().GetProperty(pt.Name).GetValue(this, null);
                    var objVal = prizeModel.GetValue(resultModel);
                    var val = objVal?.ToString();
                    if (IsInvalidPrize(val))
                    {
                        var selector = string.Empty;
                        foreach (var prizeSelector in prizeSelectors)
                        {
                            if (prizeSelector.Name == prizeModel.Name)
                            {
                                var selectorObj = prizeSelector.GetValue(domSelector);
                                selector = selectorObj?.ToString();
                                var sources = GetCrawlSources(selector);
                                var newValue = SetNewPrizeValue(document, sources);
                                prizeModel.SetValue(resultModel, newValue);
                                if (!hasUpdated)
                                {
                                    hasUpdated = true;
                                }                                    
                            }
                        }
                    }
                }

                if (hasUpdated)
                {
                    UpdateNewValuesToLotteryResult(result, resultModel);
                    _context.Lotteries.Update(result);
                    var updateSuccessfully = _context.SaveChanges();
                    if (updateSuccessfully > 0)
                    {
                        await sendHubService.SendDataToHub(resultModel);
                    }                  
                }
            }
            await Task.FromResult(0);
        }

        private bool IsInvalidPrize(string rs)
        {
            return rs == "... " || rs == " ... " || rs == "*" || rs == "&nbsp;" || rs == "-" || rs == "" || rs == "..." || rs == "--" || string.IsNullOrEmpty(rs) || string.IsNullOrWhiteSpace(rs) || rs.Contains(".");           
        }

        private Dictionary<string, string> GetCrawlSources(string pz)
        {
            var sources = new Dictionary<string, string>();
            for (int i = 9; i <= 30; i++)
            {
                sources.Add($"http://ketqua{i}.net/", pz);
            }
            return sources;
        }

        private string SetNewPrizeValue(HtmlDocument document, Dictionary<string, string> sources)
        {
            var prize = "";
            var hasJustGetValue = false;
            while (!hasJustGetValue)
            {
                foreach (KeyValuePair<string, string> source in sources)
                {
                    prize = CrawlingPrizeValue(document, source.Key, source.Value);
                    if (!string.IsNullOrEmpty(prize) && !string.IsNullOrWhiteSpace(prize) && prize != "*" && prize != "&nbsp;" && prize != "-" && prize != "" && prize != "..." && prize != "--")
                    {
                        hasJustGetValue = true;
                        break;
                    }
                }
            }
            return prize;
        }

        private string CrawlingPrizeValue(HtmlDocument document, string DataLink, string position)
        {
            string result = "";
            try
            {
                WebRequest webRequest = WebRequest.Create(DataLink);
                WebResponse webResponse = webRequest.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                Encoding encode = Encoding.GetEncoding("utf-8");
                TextReader reader = new StreamReader(dataStream, encode);
                document.Load(reader);
                var node = document.DocumentNode.QuerySelector(position);
                if (node != null)
                {
                    result = node.InnerText;
                }
                result = GetDataFrom(document, position);
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }

        public string GetDataFrom(HtmlDocument document, string cssSelector)
        {            
            var result = "";
            var node = document.DocumentNode.QuerySelector(cssSelector);
            if (node != null)
            {
                result = node.InnerText;
            }           
            return result;
        }

        private void UpdateNewValuesToLotteryResult(Lottery result, LotteryViewModel newResult)
        {
            result.SpecialPrize = newResult.SpecialPrize;
            result.Prize1st = newResult.Prize1st;
        }
    }
}
