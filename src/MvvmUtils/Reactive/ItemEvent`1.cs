using System;
namespace MvvmUtils.Reactive
{
    public class ItemEvent<T>
    {
        public ItemEvent(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }
}
