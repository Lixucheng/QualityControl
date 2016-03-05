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
                    return "合同已创建, 待生产商完善信息";
                }
                case (int) EnumTradeStatus.Finish:
                {
                    return "管控合同完成";
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
                case (int) EnumTradeStatus.SampleStart:
                {
                    return "等待抽样";
                }
                case (int) EnumTradeStatus.AlreadyApply:
                {
                    return "合同确认中";
                }
                case (int) EnumTradeStatus.FinishMakeQrCode:
                {
                    return "等待生产商接收序列码";
                }
                case (int) EnumTradeStatus.BatchSelected:
                {
                    return "批次已选择，待制作检测方案";
                }
                case (int) EnumTradeStatus.ProductInfoChecked:
                {
                    return "生产商已完善信息，等待控制中心审核";
                }
                case (int) EnumTradeStatus.ProductInfoConfirmed:
                {
                    return "控制中心审核完毕，等待生产商添加批次信息";
                }
                case (int) EnumTradeStatus.SampleReceived:
                {
                    return "抽样已经完成";
                }
                case (int) EnumTradeStatus.SgsReceivedSample:
                {
                    return "等待检测中心接受样品";
                }
            }
            return "";
        }
    }
}