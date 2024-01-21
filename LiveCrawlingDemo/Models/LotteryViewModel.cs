using System.ComponentModel.DataAnnotations;

namespace LiveCrawlingDemo.Models
{
    public class LotteryViewModel
    {
        public int? Id { get; set; }
        public string Date { set; get; }
        [StringLength(20)]
        public string? SpecialPrize { set; get; }

        [StringLength(20)]
        public string? Prize1st { get; set; }
    }
}
