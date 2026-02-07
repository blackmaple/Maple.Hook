using System.Timers;

namespace Maple.Hook.Abstractions
{
    public interface IHookFactory :   IDisposable
    {
        static Dictionary<string, IHookItem> Hooks { get; } = [];
        IHookItem Create(string key,nint pTarget, nint pDetour);
 
        bool Remove(IHookItem hook);

        bool Clear();
    }


    //public interface IHookFactory<TITEM> : IDisposable
    //    where TITEM : IHookItem
    //{
    //    TITEM Create(nint pTarget, nint pDetour);
    //    bool Remove(TITEM hook);

    //    bool Clear();
    //}

    public interface IHookContext<T>
    { 
        T Value { get; }
        IHookItem HookItem { get; }
    }
}
