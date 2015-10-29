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
                    return "订单已创建, 待制作方案";
                }
                case (int) EnumTradeStatus.Finish:
                {
                    return "质量检测完成";
                }
                case (int) EnumTradeStatus.MakeQrCode:
                {
                    return "开始制码";
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
                case (int)EnumTradeStatus.SampleStart:
                {
                    return "等待抽样";
                }
                case (int)EnumTradeStatus.AlreadyApply:
                {
                    return "合同确认中";
                }
                case (int)EnumTradeStatus.FinishMakeQrCode:
                    {
                        return "等待生产商接收控制码";
                    }
            }
            return "";
        }
    }
}