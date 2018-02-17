using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace MvvmUtils
{
	public class ViewModelBase : ValidatableObject
	{
		public ViewModelBase()
		{
			
		}

        public ViewModelBase(IMessageBus messageBus)
        {
            MessageBus = messageBus;
        }

		public IMessageBus MessageBus { get; }

		public virtual void CleanUp()
		{

		}
	}
}
