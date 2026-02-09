using System.Runtime.InteropServices;

namespace Maple.Hook.Imp.Dobby.Dynamic
{
    [StructLayout(LayoutKind.Sequential)]
    readonly unsafe struct PDobbyHook(nint ptr)
    {
        public const string Name = "DobbyHook";
        [MarshalAs(UnmanagedType.SysInt)]
        public readonly delegate* unmanaged[Cdecl, SuppressGCTransition]<nint, nint, nint, EnumDobbyHookResult> _ptr = (delegate* unmanaged[Cdecl, SuppressGCTransition]<nint, nint, nint, EnumDobbyHookResult>)ptr;

        public EnumDobbyHookResult Delegate(nint address, nint replace_func, nint porigin_func) => _ptr(address, replace_func, porigin_func);
        public static implicit operator nint(PDobbyHook dobbyHook) => (nint)dobbyHook._ptr;
        public static implicit operator PDobbyHook(nint ptr) => new(ptr);

    }


}
