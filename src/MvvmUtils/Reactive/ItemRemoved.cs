namespace MvvmUtils.Reactive
{
    public class ItemRemoved<T>
    {
        public ItemRemoved(int index, T item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; }

        public T Item { get; }
    }
}
