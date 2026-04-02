namespace Maple.Hook.Abstractions
{
    public unsafe class HookJumpChainResolver
    {
        private const int DefaultMaxResolveDepth = 32;

        /// <summary>
        /// 跳过 JMP 链，找到真正的原始函数地址
        /// </summary>
        /// <param name="pbCode">函数入口地址</param>
        /// <returns>真正的原始函数地址</returns>
        public static nint SkipJumpChain(nint pbCode, int maxResolveDepth = DefaultMaxResolveDepth)
        {
            if (pbCode == nint.Zero)
            {
                return nint.Zero;
            }

            if (maxResolveDepth <= 0)
            {
                maxResolveDepth = DefaultMaxResolveDepth;
            }

            nint current = pbCode;

            for (int depth = 0; depth < maxResolveDepth; depth++)
            {
                byte* code = (byte*)current;
                if (!TryGetJumpTarget(current, code, out nint next) || next == 0 || next == current)
                {
                    return current;
                }

                current = next;
            }

            return current;
        }
        public static bool TrySkipJumpChain(nint pbCode, out nint pRealAddress, int maxResolveDepth = DefaultMaxResolveDepth)
        { 
            pRealAddress = SkipJumpChain(pbCode, maxResolveDepth);
            return pRealAddress != nint.Zero;
        }
        private static bool TryGetJumpTarget(nint address, byte* code, out nint target)
        {
            switch (code[0])
            {
                // x86/x64: jmp rel32 (E9)
                case 0xE9:
                    target = address + 5 + *(int*)(code + 1);
                    return true;

                // x86/x64: jmp rel8 (EB)
                case 0xEB:
                    target = address + 2 + *(sbyte*)(code + 1);
                    return true;

                // x64: jmp [rip+disp32] / x86: jmp dword ptr [addr32] (FF 25)
                case 0xFF when code[1] == 0x25:
                    nint pointerAddress = IntPtr.Size == 8
                        ? address + 6 + *(int*)(code + 2)
                        : *(int*)(code + 2);
                    target = *(nint*)pointerAddress;
                    return true;

                // x64: mov rax, imm64; jmp rax
                case 0x48 when IntPtr.Size == 8 && code[1] == 0xB8 && code[10] == 0xFF && code[11] == 0xE0:
                    target = *(nint*)(code + 2);
                    return true;

                // x86: mov eax, imm32; jmp eax
                case 0xB8 when IntPtr.Size == 4 && code[5] == 0xFF && code[6] == 0xE0:
                    target = *(int*)(code + 1);
                    return true;
            }

            target = 0;
            return false;
        }
    }
}
