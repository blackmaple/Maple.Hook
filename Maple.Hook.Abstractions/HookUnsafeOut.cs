using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Maple.Hook.Abstractions
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct HookUnsafeOut<T>(scoped ref T data) where T : unmanaged
    {
        [MarshalAs(UnmanagedType.SysInt)]
        readonly nint _ptr = new(Unsafe.AsPointer(ref data));

        //public ref T Raw => ref Unsafe.AsRef<T>(_ptr.ToPointer());

        public static HookUnsafeOut<T> FromOut(out T data)
        {
            Unsafe.SkipInit(out data);
            return new HookUnsafeOut<T>(ref data);
        }


        public static implicit operator nint(HookUnsafeOut<T> data)=> data._ptr;
    }
}
