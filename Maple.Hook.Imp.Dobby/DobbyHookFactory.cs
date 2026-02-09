using Maple.Hook.Abstractions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Maple.Hook.Imp.Dobby
{
    class DobbyHookFactory(IDobbyHookRuntime runtime) : IHookFactory, IDisposable
    {
        IDobbyHookRuntime Runtime { get; } = runtime;

        public bool Clear()
        {
            foreach (var hook in IHookFactory.Hooks)
            {
                this.Remove(hook.Value);
            }
            IHookFactory.Hooks.Clear();
            return true;
        }

        public THookItem Create<THookItem>(string key, nint pTarget, nint pDetour) where THookItem : HookItem, new()
        {

            if (IHookFactory.TryGet<THookItem>(key, out var hookItem))
            {
                return hookItem;
            }
            var status = this.Runtime.DobbyPrepare(pTarget, pDetour, out nint pOriginal);
            if (status != EnumDobbyHookResult.Success)
            {
                return HookException.Throw<THookItem>($"DobbyHook Error:{status}");
            }
            hookItem = new THookItem()
            {
                Factory = this,
                Key = key,
                TargetPointer = pTarget,
                DetourPointer = pDetour,
                OriginalPointer = pOriginal,
                State = EnumHookItemState.Disabled,

            };
            if (!IHookFactory.TryAdd(key, hookItem))
            {
                return HookException.Throw<THookItem>("DobbyHook Error:create failure");
            }
            return hookItem;
        }

        public bool Disable<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            return this.Remove(hookItem);
        }

        public void Dispose()
        {
            this.Clear();
            GC.SuppressFinalize(this);
        }

        public bool Enable<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            if (hookItem.State == EnumHookItemState.Removed)
            {
                return HookException.Throw<bool>(hookItem.State);
            }
            var status = this.Runtime.DobbyCommit(hookItem.TargetPointer);
            if (status == EnumDobbyHookResult.Success)
            {
                hookItem.State = EnumHookItemState.Enabled;
                return true;
            }
            return HookException.Throw<bool>($"DobbyHook Error:{status}");

        }

        public bool Remove<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            if (hookItem.State == EnumHookItemState.Removed)
            {
                return true;
            }
            var status = this.Runtime.DobbyDestroy(hookItem.TargetPointer);
            if (status == EnumDobbyHookResult.Success)
            {
                hookItem.State = EnumHookItemState.Removed;
                IHookFactory.TryRemove(hookItem.Key);
                return true;
            }
            return HookException.Throw<bool>($"DobbyHook Error:{status}");
        }

    }
}
