using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MvvmUtils.Reactive
{
    public class ReactiveOrderedCollection<T> : ReactiveCollectionBase<T>
    {
		private IComparer<T> comparer;

		public ReactiveOrderedCollection()
		{
			this.comparer = Comparer<T>.Default;
		}

		public ReactiveOrderedCollection(IComparer<T> comparer)
        {
			this.comparer = comparer;
        }

        public override int Add(object item)
        {
            var index = InsertItem((T)item);
			whenItemInserted.OnNext(new ItemInserted<T>(index, (T)item));
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
			collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, index));
            return index;
        }

        public override void Add(T item)
        {
            var index = InsertItem(item);
			whenItemInserted.OnNext(new ItemInserted<T>(index, item));
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, index));
        }

        public override void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public override T this[int index]
        {
			get => base[index];
            set => throw new NotSupportedException();
        }

        private int InsertItem(T item)
        {
            if (source.Count == 0)
            {
                source.Add(item);
                return 0;
            }
			if (comparer.Compare(source[source.Count - 1], item) <= 0)
            {
                source.Add(item);
				return Count;
            }
			if (comparer.Compare(source[0], item) >= 0)
            {
                source.Insert(0, item);
                return 0;
            }
            int index = source.BinarySearch(item);
            if (index < 0)
                index = ~index;
            source.Insert(index, item);
			return index;
        }
    }
}
