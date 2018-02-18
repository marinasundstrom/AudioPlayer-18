namespace MvvmUtils.Reactive
{
    public class ItemReplaced<T> : ItemEvent<T>
    {
        public ItemReplaced(int index, T newItem, T oldItem) : base(index)
        {
            NewItem = newItem;
            OldItem = oldItem;
        }

        public T NewItem { get; }
        public T OldItem { get; }
    }
}
