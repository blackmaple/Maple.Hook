

using Maple.Hook.Abstractions;
using Maple.Hook.Imp.Dobby.Dynamic;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Test_Sum;


var services = new ServiceCollection();

services.AddDobbyHookDynamicFactory("C:\\Users\\NBNN_IT_CODE\\source\\repos\\Maple.Hook\\Maple.Hook.Imp.Dobby.Dynamic\\build\\runtimes\\win-x64\\dobby.dll");
//services.AddDobbyHookNativeFactory();

var app = services.BuildServiceProvider();

var factory = app.GetRequiredService<IHookFactory>();
unsafe
{


    //delegate* unmanaged[Stdcall]<int, int, int> sumPtr = &Test_Sum.Sum;
    //delegate* unmanaged[Stdcall]<int, int, int> hook_sumPtr = &Test_Sum.Hook_Sum;
    Console.WriteLine($"sum=>{Test_Sum.MethodPointer_Sum.Delegate(1, 2)}");
    delegate* unmanaged[Stdcall]<int, int, int> hook_ptr = &Test_Sum.Hook_Sum;
    using var hookItem = factory.Create<Test_Sum, Ptr_Sum, Ptr_HookSum>(new Test_Sum.Ptr_Sum(), new Test_Sum.Ptr_HookSum());

    Console.WriteLine($"sum=>{Test_Sum.MethodPointer_Sum.Delegate(1, 2)}");
    hookItem.Enable();
    Console.WriteLine($"sum=>{Test_Sum.MethodPointer_Sum.Delegate(1, 2)}");

    hookItem.Disable();
    Console.WriteLine($"sum=>{Test_Sum.MethodPointer_Sum.Delegate(1, 2)}");
    hookItem.Enable();
    Console.WriteLine($"sum=>{Test_Sum.MethodPointer_Sum.Delegate(1, 2)}");

}

Console.Read();


class Test_Sum : HookItem<Test_Sum, Test_Sum.Ptr_Sum, Test_Sum.Ptr_HookSum>
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct Ptr_Sum() : IHookMethod
    {
        //  [MarshalAs(UnmanagedType.SysInt)]
        readonly delegate* unmanaged[Stdcall]<int, int, int> m_Pointer = &Sum;
        public nint PtrMethod => (nint)m_Pointer;

        public static implicit operator nint(Ptr_Sum ptr) => ptr.PtrMethod;

        public int Delegate(int a, int b) => m_Pointer(a, b);
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct Ptr_HookSum() : IHookMethod
    {
        //  [MarshalAs(UnmanagedType.SysInt)]
        readonly delegate* unmanaged[Stdcall]<int, int, int> m_Pointer = &Hook_Sum;

        public nint PtrMethod => (nint)m_Pointer;

        public static implicit operator nint(Ptr_HookSum ptr) => ptr.PtrMethod;
        public int Delegate(int a, int b) => m_Pointer(a, b);

    }

    public static Ptr_Sum MethodPointer_Sum => new();
    public static Ptr_HookSum MethodPointer_HookSum => new();

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    public static int Sum(int a, int b) => a + b;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    public static int Hook_Sum(int a, int b)
    {
        if (Test_Sum.TryGet(  out var hookItem))
        {

            var sum = hookItem.OriginalMethod.Delegate(a, b);
            Console.WriteLine($"HOOK:var sum = {a} + {b} = {sum}");
            return sum;
        }
        else
        {
            Console.WriteLine($"NOT FOUND HOOKITEM");
            return default;
        }

    }

}

