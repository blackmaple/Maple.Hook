using System.Diagnostics.CodeAnalysis;
namespace Maple.Hook.Abstractions
{
    public class  HookException(string? msg)
        : Exception(msg)
    {

 

        [DoesNotReturn]
        public static void Throw(string msg) => throw new HookException(msg);
        [DoesNotReturn]
        public static T Throw<T>(string msg) => throw new HookException(msg);

        [DoesNotReturn]
        public static void Throw(EnumHookItemState status) => throw new HookException($"{nameof(HookItem)} Error:{status}");

        [DoesNotReturn]
        public static T Throw<T>(EnumHookItemState status) => throw new HookException($"{nameof(HookItem)} Error:{status}");

    }


}
