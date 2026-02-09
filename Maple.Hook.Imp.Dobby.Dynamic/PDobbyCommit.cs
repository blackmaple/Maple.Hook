using System.Runtime.InteropServices;

namespace Maple.Hook.Imp.Dobby.Dynamic
{
    [StructLayout(LayoutKind.Sequential)]
    readonly unsafe struct PDobbyCommit(nint ptr)
    {
        public const string Name = "DobbyCommit";

        [MarshalAs(UnmanagedType.SysInt)]
        public readonly delegate* unmanaged[Cdecl, SuppressGCTransition]<nint, EnumDobbyHookResult> _ptr = (delegate* unmanaged[Cdecl, SuppressGCTransition]<nint, EnumDobbyHookResult>)ptr;

        public EnumDobbyHookResult Delegate(nint address) => _ptr(address);

        public static implicit operator nint(PDobbyCommit dobbyHook) => (nint)dobbyHook._ptr;
        public static implicit operator PDobbyCommit(nint ptr) => new(ptr);

    }
}
