using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MH_STATUS = Maple.Hook.Imp.MinHook.EnumMinHookStatus;
namespace Maple.Hook.Imp.MinHook
{
    internal partial class MinHookNativeMethods
    {
        public static bool Initialize { get; set; }

        static MinHookNativeMethods()
        {
            Initialize = MH_Initialize() == MH_STATUS.MH_OK;
        }

        // 我们直接链接 MinHook 静态库，所以使用 "__Internal"
        private const string LibraryName = "libMinHook.x64";

        /// <summary>
        /// 初始化 MinHook
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_Initialize")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_Initialize();

        /// <summary>
        /// 清理 MinHook
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_Uninitialize")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_Uninitialize();

        /// <summary>
        /// 创建钩子
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_CreateHook")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_CreateHook(
            nint pTarget,
            nint pDetour,
              nint ppOriginal);

        /// <summary>
        /// 创建 API 钩子
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_CreateHookApi")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_CreateHookApi(
            [MarshalAs(UnmanagedType.LPWStr)] string pszModule,
            [MarshalAs(UnmanagedType.LPStr)] string pszProcName,
            nint pDetour,
            out nint ppOriginal);

        /// <summary>
        /// 创建 API 钩子（扩展）
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_CreateHookApiEx")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_CreateHookApiEx(
            [MarshalAs(UnmanagedType.LPWStr)] string pszModule,
            [MarshalAs(UnmanagedType.LPStr)] string pszProcName,
            nint pDetour,
            out nint ppOriginal,
            out nint ppTarget);

        /// <summary>
        /// 启用钩子
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_EnableHook")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_EnableHook(nint pTarget);

        /// <summary>
        /// 禁用钩子
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_DisableHook")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_DisableHook(nint pTarget);

        /// <summary>
        /// 状态码转字符串
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_StatusToString")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static partial string MH_StatusToString(MH_STATUS status);

        /// <summary>
        /// 启用所有钩子
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_EnableAllHooks")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_EnableAllHooks();

        /// <summary>
        /// 禁用所有钩子
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "MH_DisableAllHooks")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_DisableAllHooks();

        /// <summary>
        /// 移除勾子
        /// </summary>
        /// <param name="pTarget"></param>
        /// <returns></returns>
        [LibraryImport(LibraryName, EntryPoint = "MH_RemoveHook")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_RemoveHook(IntPtr pTarget);


        [LibraryImport(LibraryName, EntryPoint = "MH_QueueEnableHook")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_QueueEnableHook(IntPtr pTarget);

        [LibraryImport(LibraryName, EntryPoint = "MH_QueueDisableHook")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_QueueDisableHook(IntPtr pTarget);

        [LibraryImport(LibraryName, EntryPoint = "MH_ApplyQueued")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial MH_STATUS MH_ApplyQueued();
    }
}
