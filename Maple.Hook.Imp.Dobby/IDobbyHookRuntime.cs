using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.Hook.Imp.Dobby
{
    public interface IDobbyHookRuntime
    {
        EnumDobbyHookResult DobbyPrepare(nint address, nint replace_func, out nint porigin_func);
        EnumDobbyHookResult DobbyCommit(nint address);
        EnumDobbyHookResult DobbyDestroy(nint address);
    }
}
