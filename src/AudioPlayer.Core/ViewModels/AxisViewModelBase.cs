using System;
using System.Reactive.Linq;
using AudioPlayer.Services;
using MvvmUtils;

namespace AudioPlayer
{
	public class AxisViewModelBase : ViewModelBase
	{
		public AxisViewModelBase(IAppContext context, IMessageBus messageBus)
			: base(messageBus)
        {
			Context = context;
		}

        public IAppContext Context { get; }
	}
}
