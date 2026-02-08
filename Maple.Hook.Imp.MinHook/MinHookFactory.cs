using Maple.Hook.Abstractions;
using System.Collections.Concurrent;
using MH_STATUS = Maple.Hook.Imp.MinHook.EnumMinHookStatus;
namespace Maple.Hook.Imp.MinHook
{
    internal sealed class MinHookFactory : IHookFactory, IDisposable
    {
        public THookItem Create<THookItem>(string key, nint pTarget, nint pDetour) where THookItem : HookItem, new()
        {

            if (!MinHookNativeMethods.Initialize)
            {
                return HookException.Throw<THookItem>("MinHook Error:initialization failure");
            }
            if (IHookFactory.TryGet<THookItem>(key, out var hookItem))
            {
                return hookItem;
            }
            var status = MinHookNativeMethods.MH_CreateHook(pTarget, pDetour, HookUnsafeOut<nint>.FromOut(out nint pOriginal));
            if (status != MH_STATUS.MH_OK)
            {
                return HookException.Throw<THookItem>($"MinHook Error:{MinHookNativeMethods.MH_StatusToString(status)}");
            }
            hookItem = new THookItem()
            {
                Factory = this,
                Key = key,
                TargetPointer = pTarget,
                DetourPointer = pDetour,
                OriginalPointer = pOriginal
            };
            if (!IHookFactory.TryAdd(key, hookItem))
            {
                return HookException.Throw<THookItem>("MinHook Error:create failure");
            }
            return hookItem;
        }
        public bool Enable<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            if (hookItem.State == EnumHookItemState.Removed)
            {
                return HookException.Throw<bool>(hookItem.State);
            }
            if (hookItem.State == EnumHookItemState.Enabled)
            {
                return true;
            }
            var status = MinHookNativeMethods.MH_EnableHook(hookItem.TargetPointer);
            if (status == MH_STATUS.MH_OK)
            {
                hookItem.State = EnumHookItemState.Enabled;
                return true;
            }
            return HookException.Throw<bool>($"MinHook Error:{MinHookNativeMethods.MH_StatusToString(status)}");
        }
        public bool Disable<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            if (hookItem.State == EnumHookItemState.Removed)
            {
                return HookException.Throw<bool>(hookItem.State);
            }
            if (hookItem.State == EnumHookItemState.Disabled)
            {
                return true;
            }
            var status = MinHookNativeMethods.MH_DisableHook(hookItem.TargetPointer);
            if (status == MH_STATUS.MH_OK)
            {
                hookItem.State = EnumHookItemState.Disabled;
                return true;
            }
            return HookException.Throw<bool>($"MinHook Error:{MinHookNativeMethods.MH_StatusToString(status)}");
        }
        public bool Remove<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            if (hookItem.State == EnumHookItemState.Removed)
            {
                return true;
            }
            
            var status = MinHookNativeMethods.MH_RemoveHook(hookItem.TargetPointer);
            if (status == MH_STATUS.MH_OK)
            {
                hookItem.State = EnumHookItemState.Removed;
                IHookFactory.TryRemove(hookItem.Key);
                return true;
            }

            return HookException.Throw<bool>($"MinHook Error:{MinHookNativeMethods.MH_StatusToString(status)}");
        }
        public bool Clear()
        {
            if (!MinHookNativeMethods.Initialize)
            {
                return HookException.Throw<bool>("MinHook Error:initialization failure");
            }
            foreach (var hook in IHookFactory.Hooks)
            {
                this.Remove(hook.Value);
            }
            IHookFactory.Hooks.Clear();
            return true;
        }

        public void Dispose()
        {
            this.Clear();
            GC.SuppressFinalize(this);
        }

    }
}
