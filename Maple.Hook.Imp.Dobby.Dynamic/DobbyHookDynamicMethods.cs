using Maple.Hook.Abstractions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Maple.Hook.Imp.Dobby.Dynamic
{
    sealed class DobbyHookDynamicMethods(
        nint handle,
        PDobbyDestroy pDobbyDestroy,
        PDobbyCommit pDobbyCommit,
        PDobbyPrepare pDobbyPrepare
        ) : IDobbyHookRuntime, IDisposable
    {
        nint Handle { get; set; } = handle;
        //public required PDobbyHook DobbyHook { get; init; }
        PDobbyDestroy Ptr_DobbyDestroy { get; } = pDobbyDestroy;
        PDobbyCommit Ptr_DobbyCommit { get; } = pDobbyCommit;
        PDobbyPrepare Ptr_DobbyPrepare { get; } = pDobbyPrepare;

        public EnumDobbyHookResult DobbyCommit(nint address) => Ptr_DobbyCommit.Delegate(address);
        public EnumDobbyHookResult DobbyDestroy(nint address) => Ptr_DobbyDestroy.Delegate(address);
        public EnumDobbyHookResult DobbyPrepare(nint address, nint replace_func, out nint porigin_func)
            => Ptr_DobbyPrepare.Delegate(address, replace_func, HookUnsafeOut<nint>.FromOut(out porigin_func));



        public static IDobbyHookRuntime Create(string dll)
        {
            if (!System.IO.File.Exists(dll))
            {
                return HookException.Throw<IDobbyHookRuntime>($"NOT FOUND:{dll}");
            }
            if (!NativeLibrary.TryLoad(dll, out var handle))
            {
                return HookException.Throw<IDobbyHookRuntime>($"NOT LOADED:{dll}");
            }
            //if (!NativeLibrary.TryGetExport(handle, PDobbyHook.Name, out var pDobbyHook))
            //{
            //    return HookException.Throw<DobbyHookFactory>($"EXPORT:{PDobbyHook.Name}");
            //}
            if (!NativeLibrary.TryGetExport(handle, PDobbyDestroy.Name, out var pDobbyDestroy))
            {
                return HookException.Throw<IDobbyHookRuntime>($"EXPORT:{PDobbyDestroy.Name}");
            }
            if (!NativeLibrary.TryGetExport(handle, PDobbyCommit.Name, out var pDobbyCommit))
            {
                return HookException.Throw<IDobbyHookRuntime>($"EXPORT:{PDobbyCommit.Name}");
            }
            if (!NativeLibrary.TryGetExport(handle, PDobbyPrepare.Name, out var pDobbyPrepare))
            {
                return HookException.Throw<IDobbyHookRuntime>($"EXPORT:{PDobbyPrepare.Name}");
            }
            return new DobbyHookDynamicMethods(handle, pDobbyDestroy, pDobbyCommit, pDobbyPrepare);
        }

        public void Dispose()
        {
            if (this.Handle == nint.Zero)
            {
                return;
            }
            NativeLibrary.Free(Handle);
            this.Handle = nint.Zero;
            GC.SuppressFinalize(this);

        }
    }
}
