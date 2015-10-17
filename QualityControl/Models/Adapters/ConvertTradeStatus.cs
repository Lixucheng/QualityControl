using QualityControl.Db;

namespace QualityControl.Models.Adapters
{
    public static class ConvertTradeStatus
    {
        public static string GetStatusName(int x)
        {
            switch (x)
            {
                case (int) EnumTradeStatus.Create:
                {
                    return "订单已创建";
                }
                case (int) EnumTradeStatus.Finish:
                {
                    return "交易完成";
                }
                case (int) EnumTradeStatus.MakeQrCode:
                {
                    return "制码流程";
                }
                case (int) EnumTradeStatus.Testing:
                {
                    return "抽样完成, 检测中";
                }
                case (int) EnumTradeStatus.Tested:
                {
                    return "检测完成";
                }
                case (int) EnumTradeStatus.Signed:
                {
                    return "合同已签订";
                }
                case (int) EnumTradeStatus.EnsureContract:
                {
                    return "合同确认中";
                }
            }
            return "";
        }
    }
}