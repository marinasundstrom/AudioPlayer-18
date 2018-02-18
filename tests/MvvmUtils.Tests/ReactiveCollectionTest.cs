using System;
using Xunit;
using MvvmUtils;
using MvvmUtils.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;

namespace MvvmUtils.Tests
{
    public class ReactiveCollectionTest
	{
		[Fact]
		public void AddAndInsert_Successful()
		{
			var collection = new ReactiveCollection<string>();
			collection.WhenItemInserted.Subscribe(ev => Console.WriteLine($"{ev.Index} {ev.Item}"));
			collection.Add("Item 1");

            Assert.Equal("Item 1", collection[0]);

			collection.Add("Item 3");

            Assert.Equal("Item 3", collection[1]);

			collection.Insert(1, "Item 2");

            Assert.Equal("Item 2", collection[1]);
            Assert.Equal("Item 3", collection[2]);

			collection.Add("Item 4");

			Assert.Equal("Item 1", collection[0]);
            Assert.Equal("Item 2", collection[1]);
            Assert.Equal("Item 3", collection[2]);
			Assert.Equal("Item 4", collection[3]);
        }
    }
}
