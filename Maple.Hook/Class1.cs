

using Maple.Hook.Abstractions;
using Maple.Hook.Imp.MinHook;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


var services = new ServiceCollection();

services.AddMinHookFactory();

var app = services.BuildServiceProvider();

var factory = app.GetRequiredService<IHookFactory>();
unsafe
{


    delegate* unmanaged[Stdcall]<int, int, int> sumPtr = &Sum;
    delegate* unmanaged[Stdcall]<int, int, int> hook_sumPtr = &Hook_Sum;
    Console.WriteLine($"sum=>{sumPtr(1, 2)}");

    using var hookItem = factory.Create((nint)sumPtr, (nint)hook_sumPtr);

    Console.WriteLine($"sum=>{sumPtr(1, 2)}");
    hookItem.Enable();
    Console.WriteLine($"sum=>{sumPtr(1, 2)}");

    hookItem.Disable();
    Console.WriteLine($"sum=>{sumPtr(1, 2)}");


}

Console.Read();


[UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
static int Sum(int a, int b) => a + b;

[UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
static int Hook_Sum(int a, int b)
{
    var sum =    a + b + a + b;
    Console.WriteLine($"var sum = {a} + {b} + {a} + {b} = {sum}");
    return sum;
}