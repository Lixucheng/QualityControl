using QualityControl.Db;

namespace QualityControl.Models.Adapters
{
    public class TradeInfo
    {
        public Trade Trade { get; set; }

        public ApplicationUser User { get; set; }
    }
}