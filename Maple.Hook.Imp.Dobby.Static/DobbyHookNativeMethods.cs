using Maple.Hook.Abstractions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Maple.Hook.Imp.Dobby.Static
{
    internal partial class DobbyHookNativeMethods : IDobbyHookRuntime
    {
        // 我们直接链接 MinHook 静态库，所以使用 "__Internal"
        private const string LibraryName = "dobby";

        /// <summary>
        /// 提交 Hook
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "DobbyCommit")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl),typeof(CallConvSuppressGCTransition)])]
        public static partial EnumDobbyHookResult PtrDobbyCommit(nint address);

        /// <summary>
        /// 清理 Hook
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "DobbyDestroy")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
        public static partial EnumDobbyHookResult PtrDobbyDestroy(nint address);

        /// <summary>
        /// 预 Hook
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "DobbyPrepare")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
        public static partial EnumDobbyHookResult PtrDobbyPrepare(nint address, nint replace_func, nint porigin_func);


        public EnumDobbyHookResult DobbyCommit(nint address) => PtrDobbyCommit(address);
        public EnumDobbyHookResult DobbyDestroy(nint address) => PtrDobbyDestroy(address);
        public EnumDobbyHookResult DobbyPrepare(nint address, nint replace_func, out nint porigin_func)
            => PtrDobbyPrepare(address, replace_func, HookUnsafeOut<nint>.FromOut(out porigin_func));

    }
}
