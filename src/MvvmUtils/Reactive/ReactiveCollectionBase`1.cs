using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace MvvmUtils.Reactive
{
    public abstract class ReactiveCollectionBase<T> : IList, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        protected object _syncRoot = new object();

        protected readonly List<T> source;

        protected ISubject<ItemInserted<T>> whenItemInserted;
        protected ISubject<ItemRemoved<T>> whenItemRemoved;
        protected ISubject<ItemReplaced<T>> whenItemReplaced;

        protected NotifyCollectionChangedEventHandler collectionChanged;
        protected PropertyChangedEventHandler propertyChanged;

        public ReactiveCollectionBase()
        {
            source = new List<T>();

            whenItemInserted = new Subject<ItemInserted<T>>();
            whenItemRemoved = new Subject<ItemRemoved<T>>();
            whenItemReplaced = new Subject<ItemReplaced<T>>();
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => propertyChanged += value;
            remove => propertyChanged -= value;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IObservable<ItemInserted<T>> WhenItemInserted => whenItemInserted;

        public IObservable<ItemRemoved<T>> WhenItemRemoved => whenItemRemoved;

        public IObservable<ItemReplaced<T>> WhenItemReplaced => whenItemReplaced;

        public virtual T this[int index]
        {
            get => source[index];
            set
            {
                var oldItem = source[index];
                source[index] = value;
                whenItemReplaced.OnNext(new ItemReplaced<T>(index, source[index], oldItem));
                OnPropertyChanged("Item[]");
                collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new[] { value }, index));
            }
        }

        public int Count => source.Count;

        public bool IsReadOnly => false;

        public virtual void Add(T item)
        {
            source.Add(item);
            whenItemInserted.OnNext(new ItemInserted<T>(source.Count - 1, item));
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, source.Count - 1));
        }

        public void AddRange(params T[] items) 
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public virtual void Clear()
        {
            var items = source.ToArray();
            int i = 0;
            foreach (var item in items)
            {
                source.Remove(item);
                whenItemRemoved.OnNext(new ItemRemoved<T>(i++, item));
            }
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item) => source.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => source.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => source.GetEnumerator();

        public int IndexOf(T item) => source.IndexOf(item);

        public virtual void Insert(int index, T item)
        {
            source.Insert(index, item);
            whenItemInserted.OnNext(new ItemInserted<T>(index, item));
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, index));
        }

        public bool Remove(T item)
        {
            var index = source.IndexOf(item);
            var result = source.Remove(item);
            if (result)
            {
                whenItemRemoved.OnNext(new ItemRemoved<T>(index, item));
                OnPropertyChanged("Count");
                OnPropertyChanged("Item[]");
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            if (index >= 0)
            {
                var item = source[index];
                source.RemoveAt(index);
                whenItemRemoved.OnNext(new ItemRemoved<T>(index, item));
                OnPropertyChanged("Count");
                OnPropertyChanged("Item[]");
                collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new[] { item }, index));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

        public virtual int Add(object item)
        {
            try
            {
                Add((T)item);
                return source.Count - 1;
            }
            catch
            {
                return -1;
            }
        }

        public bool Contains(object value)
        {
            try
            {
                return Contains((T)value);
            }
            catch
            {
                return false;
            }
        }

        public int IndexOf(object value)
        {
            try
            {
                return IndexOf((T)value);
            }
            catch
            {
                return -1;
            }
        }

        void IList.Insert(int index, object item) => Insert(index, (T)item);

        public bool IsFixedSize => false;

        public void Remove(object item) => Remove((T)item);

        object IList.this[int index]
        {
            get => source[index];
            set
            {
                var oldItem = source[index];
                source[index] = (T)value;
                whenItemReplaced.OnNext(new ItemReplaced<T>(source.Count, source[index], oldItem));
                OnPropertyChanged("Item[]");
                collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, index));
            }
        }

        public void CopyTo(Array array, int index) => CopyTo((T[])array, index);

        public bool IsSynchronized => false;

        public object SyncRoot => _syncRoot;
    }
}
