using System;
using Xunit;
using MvvmUtils;
using MvvmUtils.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;

namespace MvvmUtils.Tests
{
    public class ReactiveOrderedCollectionTest
	{
		[Fact]
        public void Add_Successful()
		{
			var collection = new ReactiveOrderedCollection<string>(StringComparer.CurrentCulture);
			collection.WhenItemInserted.Subscribe(ev => Console.WriteLine($"{ev.Index} {ev.Item}"));
			collection.Add("sfoo");

            Assert.Equal("sfoo", collection[0]);

			collection.Add("4fdf");

            Assert.Equal("4fdf", collection[0]);
            Assert.Equal("sfoo", collection[1]);

			collection.Add("faha");

            Assert.Equal("4fdf", collection[0]);
            Assert.Equal("faha", collection[1]);
            Assert.Equal("sfoo", collection[2]);
		}
	}
}
