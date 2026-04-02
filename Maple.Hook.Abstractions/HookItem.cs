using Maple.UnmanagedExtensions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Maple.Hook.Abstractions
{


    public abstract class HookItem : IDisposable
    {
        public AdditionalContentManager AdditionalContent { get; } = new();



        [NotNull]
        public IHookFactory? Factory { get; init; }
        /// <summary>
        /// 唯一
        /// </summary>
        [NotNull]
        public string? Key { get; init; }
        /// <summary>
        /// Hook的原函数地址
        /// </summary>
        public nint TargetPointer { get; init; }
        /// <summary>
        /// 可调用的原函数地址
        /// </summary>
        public nint OriginalPointer { get; set; }
        /// <summary>
        /// Hook的函数地址
        /// </summary>
        public nint DetourPointer { get; init; }


        public EnumHookItemState State { get; set; } = EnumHookItemState.Disabled;

        public virtual bool Enable() => this.Factory.Enable(this);
        public virtual bool Disable() => this.Factory.Disable(this);
        public virtual bool Remove() => this.Factory.Remove(this);
        public virtual bool Clear()
        {
            this.AdditionalContent.Clear();
            return this.Factory.Disable(this) && this.Factory.Remove(this);
        }

        public void Dispose()
        {
            this.Clear();
            GC.SuppressFinalize(this);
        }
    }


    public abstract class HookItem<TTargetMethod, TDetourMethod> : HookItem
        where TTargetMethod : unmanaged, IHookMethod
        where TDetourMethod : unmanaged, IHookMethod
    {
        public TTargetMethod TargetMethod
        {
            get
            {
                var pointer = this.TargetPointer;
                return Unsafe.As<nint, TTargetMethod>(ref pointer);

            }
        }
        public TDetourMethod DetourMethod
        {
            get
            {
                var pointer = this.DetourPointer;
                return Unsafe.As<nint, TDetourMethod>(ref pointer);

            }
        }
        public TTargetMethod OriginalMethod
        {
            get
            {
                var pointer = this.OriginalPointer;
                return Unsafe.As<nint, TTargetMethod>(ref pointer);

            }
        }
    }



    public abstract class HookItem<THookItem, TTargetMethod, TDetourMethod>
        : HookItem<TTargetMethod, TDetourMethod>
        where THookItem : HookItem<TTargetMethod, TDetourMethod>
        where TTargetMethod : unmanaged, IHookMethod
        where TDetourMethod : unmanaged, IHookMethod
    {
        public static bool TryGet(string key, [MaybeNullWhen(false)] out THookItem hookItem)
            => IHookFactory.TryGet(key, out hookItem);
        public static bool TryGet([MaybeNullWhen(false)] out THookItem hookItem)
            => TryGet(typeof(THookItem).Name, out hookItem);

    }

}
