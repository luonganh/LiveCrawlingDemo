using System.ComponentModel.DataAnnotations;

namespace LiveCrawlingDemo.Models
{
    public class LotteryDOMViewModel
    {
        public LotteryDOMViewModel()
        {
            SpecialPrize = GetSpecialPrize();
            Prize1st = GetPrize1st();
        }

        //public int? Id { get; set; }
        //public string Date { set; get; }
        [StringLength(20)]
        public string? SpecialPrize { set; get; }

        [StringLength(20)]
        public string? Prize1st { get; set; }

        private string GetSpecialPrize()
        {
           return SpecialPrize = "#result_tab_mb #rs_0_0";
        }

        private string GetPrize1st()
        {
            return Prize1st = "#result_tab_mb #rs_1_0";
        }
    }
}
