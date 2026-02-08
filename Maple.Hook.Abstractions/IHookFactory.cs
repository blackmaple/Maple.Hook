using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Maple.Hook.Abstractions
{
    public interface IHookFactory : IDisposable
    {
        static ConcurrentDictionary<string, HookItem> Hooks { get; } = [];
        static bool TryGet(string key, [MaybeNullWhen(false)] out HookItem hookItem) => Hooks.TryGetValue(key, out hookItem);
        static bool TryGet<T>(string key, [MaybeNullWhen(false)] out T hookItem) where T : HookItem
        {
            Unsafe.SkipInit(out hookItem);
            if (Hooks.TryGetValue(key, out var item) && item is T typedItem)
            {
                hookItem = typedItem;
                return true;
            }
            return false;

        }
        static bool TryAdd(string key, HookItem hookItem) => Hooks.TryAdd(key, hookItem);
        static bool TryRemove(string key) => Hooks.TryRemove(key, out _);

        THookItem Create<THookItem>(string key, nint pTarget, nint pDetour) where THookItem : HookItem, new();
        public THookItem Create<THookItem, TTargetMethod, TDetourMethod>(string key, TTargetMethod pTarget, TDetourMethod pDetour)
            where THookItem : HookItem, new()
            where TTargetMethod : IHookMethod
            where TDetourMethod : IHookMethod
         => Create<THookItem>(key, pTarget.PtrMethod, pDetour.PtrMethod);

        public bool Enable<THookItem>(string key) where THookItem : HookItem
        {
            if (false == TryGet<THookItem>(key, out var hookItem))
            {
                return HookException.Throw<bool>($"NOT FOUND:{key}");
            }
            return this.Enable(hookItem);
        }
        public bool Disable<THookItem>(string key) where THookItem : HookItem
        {
            if (false == TryGet<THookItem>(key, out var hookItem))
            {
                return HookException.Throw<bool>($"NOT FOUND:{key}");
            }
            return this.Disable(hookItem);
        }
        public bool Remove<THookItem>(string key) where THookItem : HookItem
        {
            if (false == TryGet<THookItem>(key, out var hookItem))
            {
                return HookException.Throw<bool>($"NOT FOUND:{key}");
            }
            return this.Remove(hookItem) ;
        }
        bool Enable<THookItem>(THookItem hookItem) where THookItem : HookItem;
        bool Disable<THookItem>(THookItem hookItem) where THookItem : HookItem;
        bool Remove<THookItem>(THookItem hookItem) where THookItem : HookItem;
        bool Clear();


    }
}
