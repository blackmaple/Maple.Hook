using Maple.Hook.Abstractions;
using System.Collections.Concurrent;
using MH_STATUS = Maple.Hook.Imp.MinHook.EnumMinHookStatus;
namespace Maple.Hook.Imp.MinHook
{
    internal sealed class MinHookFactory :   IHookFactory, IDisposable
    {

        ConcurrentDictionary<nint, MinHookItem> Hooks { get; } = new ConcurrentDictionary<nint, MinHookItem>();
        public MinHookItem Create(nint pTarget, nint pDetour)
        {
            if (!MinHookNativeMethods.Initialize)
            {
                return MinHookException.Throw<MinHookItem>("MinHook Error:initialization failure");
            }
            if (this.Hooks.TryGetValue(pTarget, out var hookItem))
            {
                return hookItem;
            }
            var status = MinHookNativeMethods.MH_CreateHook(pTarget, pDetour, MinHookUnsafeOut<nint>.FromOut(out nint pOriginal));
            if (status != MH_STATUS.MH_OK)
            {
                return MinHookException.Throw<MinHookItem>(status);
            }
            hookItem = new MinHookItem(pTarget, pDetour, pOriginal);
            if (!this.Hooks.TryAdd(pTarget, hookItem))
            {
                return MinHookException.Throw<MinHookItem>("MinHook Error:create failure");
            }
            return hookItem;

        }
        public bool Remove(MinHookItem hook)
        {
            if (!hook.Clear())
            {
                return MinHookException.Throw<bool>("MinHook Error:clear failure");
            }
            if (this.Hooks.TryRemove(hook.TargetPointer, out _))
            {
                return MinHookException.Throw<bool>("MinHook Error:remove failure");
            }
            return true;
        }
        public bool Clear()
        {
            if (!MinHookNativeMethods.Initialize)
            {
                MinHookException.Throw<bool>("MinHook Error:initialization failure");
            }
            foreach (var hook in this.Hooks)
            {
                this.Remove(hook.Value);
            }
            this.Hooks.Clear();
            return true;

        }
        public void Dispose()
        {
            if (!MinHookNativeMethods.Initialize)
            {
                return;
            }
            this.Clear();
            MinHookNativeMethods.Initialize = !(MinHookNativeMethods.MH_Uninitialize() == MH_STATUS.MH_OK);
        }

        IHookItem IHookFactory.Create(nint pTarget, nint pDetour)
        {
            return Create(pTarget, pDetour);
        }

        public bool Remove(IHookItem hook)
        {
            if(hook is not MinHookItem minHookItem)
            {
                return MinHookException.Throw<bool>("MinHook Error:invalid hook item");
            }
            return Remove(minHookItem);
        }
    }
}
