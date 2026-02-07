using Maple.Hook.Abstractions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MH_STATUS = Maple.Hook.Imp.MinHook.EnumMinHookStatus;
namespace Maple.Hook.Imp.MinHook
{
    internal class MinHookItem(nint pTarget, nint pDetour, nint pOriginal) : IHookItem, IDisposable
    {


        EnumMinHookItemState State { get; set; } = EnumMinHookItemState.Disabled;
        public nint TargetPointer { get; } = pTarget;
        public nint OriginalPointer { get; } = pDetour;
        public nint DetourPointer { get; } = pOriginal;
        public nint KeyPointer => TargetPointer;


        public bool Enable()
        {
            if (this.State == EnumMinHookItemState.Removed)
            {
                return MinHookException.Throw<bool>(this.State);
            }
            if (this.State == EnumMinHookItemState.Enabled)
            {
                return true;
            }
            var status = MinHookNativeMethods.MH_EnableHook(TargetPointer);
            if (status == MH_STATUS.MH_OK)
            {
                this.State = EnumMinHookItemState.Enabled;
                return true;
            }
            return MinHookException.Throw<bool>(status);


        }
        public bool Disable()
        {
            if (this.State == EnumMinHookItemState.Removed)
            {
                return MinHookException.Throw<bool>(this.State);
            }
            if (this.State == EnumMinHookItemState.Disabled)
            {
                return true;
            }
            var status = MinHookNativeMethods.MH_DisableHook(TargetPointer);
            if (status == MH_STATUS.MH_OK)
            {
                this.State = EnumMinHookItemState.Disabled;
                return true;
            }
            return MinHookException.Throw<bool>(status);

            
        }
        public bool Remove()
        {
            if (this.State == EnumMinHookItemState.Removed)
            {
                return true;
            }
            var status = MinHookNativeMethods.MH_RemoveHook(TargetPointer);
            if (status == MH_STATUS.MH_OK)
            {
                this.State = EnumMinHookItemState.Removed;
                return true;
            }
            return MinHookException.Throw<bool>(status);
        }
        public bool Clear()
        {
            return this.Disable() && this.Remove();
        }

        public void Dispose()
        {
            this.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
