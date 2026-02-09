using System.Runtime.InteropServices;

namespace Maple.Hook.Imp.Dobby.Dynamic
{
    [StructLayout(LayoutKind.Sequential)]
    readonly unsafe struct PDobbyPrepare(nint ptr)
    {
        public const string Name = "DobbyPrepare";
        [MarshalAs(UnmanagedType.SysInt)]
        public readonly delegate* unmanaged[Cdecl, SuppressGCTransition]<nint, nint, nint, EnumDobbyHookResult> _ptr = (delegate* unmanaged[Cdecl, SuppressGCTransition]<nint, nint, nint, EnumDobbyHookResult>)ptr;

        public EnumDobbyHookResult Delegate(nint address, nint replace_func, nint porigin_func) => _ptr(address, replace_func, porigin_func);
        public static implicit operator nint(PDobbyPrepare dobbyHook) => (nint)dobbyHook._ptr;
        public static implicit operator PDobbyPrepare(nint ptr) => new(ptr);

    }
}
