using Maple.Hook.Abstractions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Maple.Hook.Imp.Dobby
{
    class DobbyHookFactory : IHookFactory, IDisposable
    {
        public required nint Handle { get; init; }
        public required PDobbyHook DobbyHook { get; init; }
        public required PDobbyDestroy DobbyDestroy { get; init; }
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
            var status = DobbyHook.Delegate(pTarget, pDetour, HookUnsafeOut<nint>.FromOut(out nint pOriginal));
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
                State = EnumHookItemState.Enabled,
                
            };
            if (!IHookFactory.TryAdd(key, hookItem))
            {
                return HookException.Throw<THookItem>("DobbyHook Error:create failure");
            }
            return hookItem;
        }

        public bool Disable<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            return true;
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
            return hookItem.State == EnumHookItemState.Enabled;
        }

        public bool Remove<THookItem>(THookItem hookItem) where THookItem : HookItem
        {
            if (hookItem.State == EnumHookItemState.Removed)
            {
                return true;
            }
            var status = DobbyDestroy.Delegate(hookItem.TargetPointer);
            if (status == EnumDobbyHookResult.Success)
            {
                hookItem.State = EnumHookItemState.Removed;
                IHookFactory.TryRemove(hookItem.Key);
                return true;
            }
            return HookException.Throw<bool>($"DobbyHook Error:{status}");
        }

        public static DobbyHookFactory Create(string dll)
        {
            if (!System.IO.File.Exists(dll))
            {
                return HookException.Throw<DobbyHookFactory>($"NOT FOUND:{dll}");
            }
            if (!NativeLibrary.TryLoad(dll, out var handle))
            {
                return HookException.Throw<DobbyHookFactory>($"NOT LOADED:{dll}");
            }
            if (!NativeLibrary.TryGetExport(handle, PDobbyHook.Name, out var pDobbyHook))
            {
                return HookException.Throw<DobbyHookFactory>($"EXPORT:{PDobbyHook.Name}");
            }
            if (!NativeLibrary.TryGetExport(handle, PDobbyDestroy.Name, out var pDobbyDestroy))
            {
                return HookException.Throw<DobbyHookFactory>($"EXPORT:{PDobbyDestroy.Name}");
            }
            return new DobbyHookFactory()
            {
                Handle = handle,
                DobbyDestroy = pDobbyDestroy,
                DobbyHook = pDobbyHook,
            };
        }
    }
}
