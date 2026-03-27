using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Maple.Hook.Abstractions
{


    public abstract class HookItem : IDisposable
    {
        Dictionary<string, WeakReference<object>> AdditionalContent { get; } = [];

        public void SetAdditionalContent(string key, object content)
        {
            this.AdditionalContent[key] = new WeakReference<object>(content);
        }
        public void SetAdditionalContent<T>(string key, T content) where T : class
        {
            this.AdditionalContent[key] = new WeakReference<object>(content);
        }
        public bool TryGetAdditionalContent(string key, [MaybeNullWhen(false)] out object content)
        {
            Unsafe.SkipInit(out content);
            return this.AdditionalContent.TryGetValue(key, out var weakReference) && weakReference.TryGetTarget(out content);
        }
        public bool TryGetAdditionalContent<T>(string key, [MaybeNullWhen(false)] out T content) where T : class
        {
            Unsafe.SkipInit(out content);
            if (TryGetAdditionalContent(key, out var weakReference))
            {
                if (weakReference is T weakReference2)
                {
                    content = weakReference2;
                    return true;
                }
            }
            return false;
        }
        private bool ClearAdditionalContent()
        {
            this.AdditionalContent.Clear();
            return true;
        }

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
        public virtual bool Clear() => this.ClearAdditionalContent() && this.Factory.Disable(this) && this.Factory.Remove(this);

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
                var pointer = this.TargetPointer;
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
