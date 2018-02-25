using System;
using System.Linq;
using Xamarin.Forms;
using Axis.AudioPlayer.Effects;
using MvvmUtils;

namespace Axis.AudioPlayer.Validation
{
	public class EntryValidationBehavior : Behavior<Entry>
	{
		private Entry _associatedObject;
		private Label validationLabel;

		protected override void OnAttachedTo(Entry bindable)
		{
			base.OnAttachedTo(bindable);
			// Perform setup       

			_associatedObject = bindable;

			_associatedObject.TextChanged += _associatedObject_TextChanged;
		}

        private void _associatedObject_TextChanged(object sender, TextChangedEventArgs e)
		{
            if (_associatedObject.BindingContext is ValidatableObject source && !string.IsNullOrEmpty(PropertyName))
            {
                Process(source);
            }
        }

		private void Process(ValidatableObject source)
		{
			if(validationLabel == null)
			{
				validationLabel = new Label
				{
					Text = string.Empty,
					TextColor = Color.Red,
					FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
				};

				var layout = _associatedObject.Parent as StackLayout;
				var index = layout.Children.IndexOf(_associatedObject);
				layout.Children.Insert(index + 1, validationLabel);
			}

			var errors = source.GetErrors(PropertyName).Cast<string>();
#pragma warning disable RCS1146 // Use conditional access.
            if (errors != null && errors.Any())
#pragma warning restore RCS1146 // Use conditional access.
            {
				var borderEffect = _associatedObject.Effects.FirstOrDefault(eff => eff is BorderEffect);
				if (borderEffect == null)
				{
					_associatedObject.Effects.Add(new BorderEffect());
				}

				_associatedObject.TextColor = Color.Red;

				validationLabel.Text = errors.First();

				/*
				if (Device.OS != TargetPlatform.Windows)
				{
					_associatedObject.BackgroundColor = Color.Red;
				}
				*/
			}
			else
			{
				var borderEffect = _associatedObject.Effects.FirstOrDefault(eff => eff is BorderEffect);
				if (borderEffect != null)
				{
					_associatedObject.Effects.Remove(borderEffect);
				}

				_associatedObject.TextColor = Color.Black;

				validationLabel.Text = string.Empty;

				/*
				if (Device.OS != TargetPlatform.Windows)
				{
					_associatedObject.BackgroundColor = Color.Default;
				}
				*/
			}
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			base.OnDetachingFrom(bindable);
			// Perform clean up

			_associatedObject.TextChanged -= _associatedObject_TextChanged;

			_associatedObject = null;
		}

		public string PropertyName { get; set; }
	}
}

