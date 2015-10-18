namespace QualityControl.Enum
{
    public enum EnumUserStatus
    {
        /// <summary>
        ///     审核不通过
        /// </summary>
        Failed = -4,

        /// <summary>
        ///     注册未审核
        /// </summary>
        UnRecognized,

        /// <summary>
        ///     提交了注册审核申请
        /// </summary>
        Requiring,

        /// <summary>
        ///     审核通过
        /// </summary>
        Normal
    }
}