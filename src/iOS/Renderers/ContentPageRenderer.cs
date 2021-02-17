using System;
using System.Collections.Generic;
using System.Linq;
using AudioPlayer.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(ContentPage), typeof(ContentPageRenderer))]

namespace AudioPlayer.iOS.Renderers
{
	public class ContentPageRenderer : PageRenderer
	{
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			var itemsInfo = (Element as ContentPage).ToolbarItems;

			if (NavigationController == null)
				return;

			var navigationItem = NavigationController.TopViewController.NavigationItem;
			var leftNativeButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
			var rightNativeButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();

			rightNativeButtons.ToList().ForEach(nativeItem =>
			{
				var info = GetButtonInfo(itemsInfo, nativeItem.Title);

				if (info == null || info.Priority == 1)
				{
					//nativeItem.Style = UIBarButtonItemStyle.Done;

					rightNativeButtons.Remove(nativeItem);
					leftNativeButtons.Add(nativeItem);
				}
			});

			navigationItem.RightBarButtonItems = rightNativeButtons.ToArray();
			navigationItem.LeftBarButtonItems = leftNativeButtons.ToArray();
		}

		private ToolbarItem GetButtonInfo(IList<ToolbarItem> items, string name)
		{
			if (string.IsNullOrEmpty(name) || items == null)
				return null;

			return items.ToList().Find(itemData => name.Equals(itemData.Name));
		}
	}
}
