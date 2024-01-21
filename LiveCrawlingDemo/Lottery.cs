using System.ComponentModel.DataAnnotations;

namespace LiveCrawlingDemo
{
    public class Lottery
    {
        public int Id { get; set; }
        public DateTime Date { set; get; }
        [StringLength(20)]
        public string? SpecialPrize { set; get; }

        [StringLength(20)]
        public string? Prize1st { get; set; }
    }
}
