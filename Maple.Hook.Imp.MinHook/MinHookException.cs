using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using MH_STATUS = Maple.Hook.Imp.MinHook.EnumMinHookStatus;
namespace Maple.Hook.Imp.MinHook
{
    public class MinHookException(string? msg)
        : Exception(msg)
    {


        [DoesNotReturn]
        internal static void Throw(MH_STATUS status) => throw new MinHookException($"{nameof(MinHookItem)} Error:{MinHookNativeMethods.MH_StatusToString(status)}");
        [DoesNotReturn]
        internal static T Throw<T>(MH_STATUS status) => throw new MinHookException($"{nameof(MinHookItem)} Error:{MinHookNativeMethods.MH_StatusToString(status)}");


        [DoesNotReturn]
        internal static void Throw(EnumMinHookItemState status) => throw new MinHookException($"{nameof(MinHookItem)} Error:{status}");

        [DoesNotReturn]
        internal static T Throw<T>(EnumMinHookItemState status) => throw new MinHookException($"{nameof(MinHookItem)} Error:{status}");


        [DoesNotReturn]
        internal static void Throw(string msg) => throw new MinHookException(msg);
        [DoesNotReturn]
        internal static T Throw<T>(string msg) => throw new MinHookException(msg);
 
 
    }

 
}
