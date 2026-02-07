namespace Maple.Hook.Abstractions
{
    public interface IHookItem : IDisposable
    {
        /// <summary>
        /// 唯一
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Hook的原函数地址
        /// </summary>
        nint TargetPointer { get; }
        /// <summary>
        /// 可调用的原函数地址
        /// </summary>
        nint OriginalPointer { get; }
        /// <summary>
        /// Hook的函数地址
        /// </summary>
        nint DetourPointer { get; }

        bool Enable();
        bool Disable();
        bool Remove();
        bool Clear();
    }
}
