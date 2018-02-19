using System;
namespace MvvmUtils.Reactive
{
    public abstract class ItemEvent<T>
    {
        public ItemEvent(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }
}
