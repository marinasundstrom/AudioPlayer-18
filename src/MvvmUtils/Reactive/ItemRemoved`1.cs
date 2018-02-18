namespace MvvmUtils.Reactive
{
    public class ItemRemoved<T> : ItemEvent<T>
    {
        public ItemRemoved(int index, T item) : base(index)
        {
            Item = item;
        }

        public T Item { get; }
    }
}
