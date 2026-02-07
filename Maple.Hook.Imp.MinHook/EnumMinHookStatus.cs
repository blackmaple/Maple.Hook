namespace Maple.Hook.Imp.MinHook
{
    public enum EnumMinHookStatus
    {
        MH_OK = 0,                          // 成功
        MH_ERROR_ALREADY_INITIALIZED,       // 已初始化
        MH_ERROR_NOT_INITIALIZED,           // 未初始化
        MH_ERROR_ALREADY_CREATED,           // 钩子已存在
        MH_ERROR_NOT_CREATED,               // 钩子不存在
        MH_ERROR_ENABLED,                   // 钩子已启用
        MH_ERROR_DISABLED,                  // 钩子已禁用
        MH_ERROR_NOT_EXECUTABLE,            // 地址不可执行
        MH_ERROR_UNSUPPORTED_FUNCTION,      // 不支持的函数
        MH_ERROR_MEMORY_ALLOC,              // 内存分配失败
        MH_ERROR_MEMORY_PROTECT,            // 内存保护失败
        MH_ERROR_MODULE_NOT_FOUND,          // 模块未找到
        MH_ERROR_FUNCTION_NOT_FOUND         // 函数未找到
    }
}
