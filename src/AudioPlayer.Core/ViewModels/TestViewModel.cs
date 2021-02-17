using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MvvmUtils;
using MvvmUtils.Reactive;

namespace AudioPlayer
{
	public class TestViewModel : ViewModelBase
	{
		private string _text;

		public TestViewModel(IMessageBus messageBus)
			: base(messageBus)
		{
			Items = new ObservableCollection<Item>();

			var saveCommandCanExecute = this.WhenAnyValue(vm => vm.Text,v => !string.IsNullOrWhiteSpace(v));

			SaveCommand = ReactiveCommand.Create(saveCommandExecute, saveCommandCanExecute);

            SaveCommand.PushOrUpdate(this, vm => vm.Items, item => item.Id);
		}

		private async Task<Item> saveCommandExecute()
		{
			return new Item()
			{
				Id = Guid.NewGuid().ToString(),
				Text = Text
			};
		}

		public ObservableCollection<Item> Items { get; }

		public string Text 
		{
			get => _text;
			set => SetProperty(ref _text, value);
		}

		public ReactiveCommand<Item> SaveCommand { get; }
	}

	public class Item
	{
		public string Id { get; set; }

		public string Text { get; set; }	
	}
}
