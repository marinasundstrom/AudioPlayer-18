using System;
namespace Axis.AudioPlayer
{
	public interface ISubmittable
	{
		bool IsSubmitEnabled { get; }
	}
}
