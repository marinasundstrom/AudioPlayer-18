namespace MvvmUtils.Reactive
{
    public class ItemInserted<T>
    {
        public ItemInserted(int index, T item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; }

        public T Item { get; }
    }
}
