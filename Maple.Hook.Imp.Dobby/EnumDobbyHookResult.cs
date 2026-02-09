namespace Maple.Hook.Imp.Dobby
{
    public enum EnumDobbyHookResult
    {
        /// <summary>
        /// Hook 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// Hook 失败，一般性错误
        /// </summary>
        Failed = -1,

        /// <summary>
        /// 内存保护失败
        /// </summary>
        MemoryProtectFailed = -2,

        /// <summary>
        /// 指令解析失败
        /// </summary>
        InstructionParseFailed = -3,

        /// <summary>
        /// 不支持的架构
        /// </summary>
        UnsupportedArchitecture = -4,

        /// <summary>
        /// 目标地址无效
        /// </summary>
        InvalidAddress = -5,

        /// <summary>
        /// Hook 已存在
        /// </summary>
        AlreadyHooked = -6,

        /// <summary>
        /// 参数无效
        /// </summary>
        InvalidParameter = -7,

        /// <summary>
        /// 符号未找到
        /// </summary>
        SymbolNotFound = -8,

        /// <summary>
        /// 内存不足
        /// </summary>
        OutOfMemory = -9,

        /// <summary>
        /// 权限不足
        /// </summary>
        AccessDenied = -10
    }
}
