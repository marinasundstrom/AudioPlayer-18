namespace MvvmUtils.Reactive
{
    public class ItemReplaced<T>
    {
        public ItemReplaced(int index, T newItem, T oldItem)
        {
            Index = index;
            NewItem = newItem;
            OldItem = oldItem;
        }

        public int Index { get; }

        public T NewItem { get; }
        public T OldItem { get; }
    }
}
