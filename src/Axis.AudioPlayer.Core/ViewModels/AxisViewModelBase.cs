using System;
using System.Reactive.Linq;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer
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
