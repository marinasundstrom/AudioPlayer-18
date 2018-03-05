using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;

namespace MvvmUtils.Reactive
{
    public static class Extensions
    {
        static Dictionary<object, Dictionary<string, object>> propertyObservableCache = new Dictionary<object, Dictionary<string, object>>();
        static Dictionary<Type, Dictionary<string, PropertyInfo>> typePropertyCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        static string BASE_KEY = "*";

        public static IObservable<TResult> WhenAnyValue<T, TResult>(this T source, Expression<Func<T, TResult>> property)
            where T : INotifyPropertyChanged
            => GetObservableForPropertyWithInitalValue(source, property);

        public static IObservable<TResult> WhenAnyValue<T, TProperty, TResult>(this T source, Expression<Func<T, TProperty>> property, Func<TProperty, TResult> selector)
            where T : INotifyPropertyChanged
        => GetObservableForPropertyWithInitalValue(source, property).Select(selector);

        public static IObservable<TResult> WhenAnyValue<T, TProp1, TProp2, TResult>(this T source, Expression<Func<T, TProp1>> property1, Expression<Func<T, TProp2>> property2, Func<TProp1, TProp2, TResult> merge)
            where T : INotifyPropertyChanged
            => GetObservableForPropertyWithInitalValue(source, property1).CombineLatest(
            GetObservableForPropertyWithInitalValue(source, property2),
            merge);

        public static IObservable<TResult> WhenAnyValue<T, TProp1, TProp2, TProp3, TResult>(this T source, Expression<Func<T, TProp1>> property1, Expression<Func<T, TProp2>> property2, Expression<Func<T, TProp3>> property3, Func<TProp1, TProp2, TProp3, TResult> merge)
            where T : INotifyPropertyChanged
        => GetObservableForPropertyWithInitalValue(source, property1).CombineLatest(
            GetObservableForPropertyWithInitalValue(source, property2),
            GetObservableForPropertyWithInitalValue(source, property3),
            merge);

        public static IObservable<TProperty> ObservableForProperty<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
            where T : INotifyPropertyChanged
        {
            if (!propertyObservableCache.TryGetValue(source, out var props))
            {
                props = new Dictionary<string, object>();
                props[BASE_KEY] = Observable
                    .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => source.PropertyChanged += h,
                        h => source.PropertyChanged -= h).AsObservable();
                propertyObservableCache[source] = props;
            }
            var propertyName = GetPropertyName(property);
            if (!props.TryGetValue(propertyName, out var observable))
            {
                observable = ((IObservable<EventPattern<PropertyChangedEventArgs>>)props[BASE_KEY])
                .Where(eventPattern => eventPattern.EventArgs.PropertyName == GetPropertyName(property))
                    .Select(eventPattern => GetValue<TProperty>(eventPattern)).AsObservable();
                props[propertyName] = observable;
            }
            return (IObservable<TProperty>)observable;
        }

        private static TResult GetValue<TResult>(EventPattern<PropertyChangedEventArgs> eventPattern)
        {
            var sender = eventPattern.Sender;
            var senderType = sender.GetType();
            var prop = eventPattern.EventArgs.PropertyName;

            if (!typePropertyCache.TryGetValue(senderType, out var props))
            {
                props = new Dictionary<string, PropertyInfo>();
                typePropertyCache[senderType] = props;
            }
            if (!props.TryGetValue(prop, out var observable))
            {
                observable = eventPattern.Sender
                                    .GetType()
                                    .GetRuntimeProperties().First(x => x.Name == prop);

                props[prop] = observable;
            }
            return (TResult)observable.GetValue(sender);
        }

        private static IObservable<TProperty> GetObservableForPropertyWithInitalValue<T, TProperty>(T source, Expression<Func<T, TProperty>> property)
            where T : INotifyPropertyChanged
            => Observable.Defer(() =>
            {
                var propertyObservable = ObservableForProperty(source, property);
                return Observable.Create<TProperty>((observer) =>
                {
                    var value = (TProperty)GetPropertyInfo(property).GetValue(source);
                    observer.OnNext(value);
                    return () => { };
                })
                .Merge(propertyObservable);
            });

        public static IDisposable SetProperty<TSource, TTarget>(this IObservable<TSource> source, TTarget obj, Expression<Func<TTarget, TSource>> property)
             => source.Subscribe((value) =>
            {
                var propertyInfo = GetPropertyInfo(property);
                propertyInfo.SetValue(obj, value);
            });

        public static IDisposable InvokeCommand<TSource, TTarget>(this IObservable<TSource> source, TTarget obj, Func<TTarget, ReactiveCommand<TSource>> commandSelector)
            => source.Subscribe((value) =>
            {
                var command = commandSelector(obj) as ICommand;
                if (command?.CanExecute(value) ?? false)
                {
                    command.Execute(value);
                }
            });

        public static IDisposable Push<TSource, TTarget>(this IObservable<TSource> source, TTarget obj, Func<TTarget, IList<TSource>> property)
            => source.Subscribe((value) => property(obj).Add(value));

        public static IDisposable PushOrUpdate<TSource, TTarget, TKey>(this IObservable<TSource> source, TTarget obj, Func<TTarget, IList<TSource>> property, Expression<Func<TSource, TKey>> keyProperty)
            => source.Subscribe((value) =>
            {
                var propertyInfo = GetPropertyInfo(keyProperty);
                var keyPropertyValue = propertyInfo.GetValue(value);
                var target = property(obj);
                var original = target.FirstOrDefault(x => object.Equals(propertyInfo.GetValue(x), keyPropertyValue));
                if (original == null)
                {
                    target.Add(value);
                }
                else
                {
                    var index = target.IndexOf(original);
                    if (target.Count >= index)
                    {
                        target[index] = value;
                    }
                }
            });

        private static PropertyInfo GetPropertyInfo<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
            => GetPropertyCore(propertyExpression).Member as PropertyInfo;

        private static MemberExpression GetPropertyCore<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression.Body is MemberExpression memberExpression
               && memberExpression.Member.MemberType == MemberTypes.Property)
            {
                return memberExpression;
            }

            throw new InvalidOperationException("Expected a property expression.");
        }

        private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
            => GetPropertyCore(propertyExpression).Member.Name;
    }
}
