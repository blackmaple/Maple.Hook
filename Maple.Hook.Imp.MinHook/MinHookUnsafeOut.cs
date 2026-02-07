using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Maple.Hook.Imp.MinHook
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly unsafe struct MinHookUnsafeOut<T>(scoped ref T data) where T : unmanaged
    {
        [MarshalAs(UnmanagedType.SysInt)]
        readonly nint _ptr = new(Unsafe.AsPointer(ref data));

        //public ref T Raw => ref Unsafe.AsRef<T>(_ptr.ToPointer());

        public static MinHookUnsafeOut<T> FromOut(out T data)
        {
            Unsafe.SkipInit(out data);
            return new MinHookUnsafeOut<T>(ref data);
        }


        public static implicit operator nint(MinHookUnsafeOut<T> data)=> data._ptr;
    }
}
