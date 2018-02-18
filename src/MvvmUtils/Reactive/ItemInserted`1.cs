namespace MvvmUtils.Reactive
{
    public class ItemInserted<T> : ItemEvent<T>
    {
        public ItemInserted(int index, T item) : base(index)
        {
            Item = item;
        }

        public T Item { get; }
    }
}
