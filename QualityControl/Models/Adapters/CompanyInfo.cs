using QualityControl.Db;

namespace QualityControl.Models.Adapters
{
    public class CompanyInfo
    {
        public Company Company { get; set; }

        public ApplicationUser User { get; set; }
    }
}