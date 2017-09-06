using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Services
{
	public class AnimationService: IDisposable
	{
		#region Constants
		private const string rotationAnimationKey = "rotationAnimation";
		#endregion

		public AnimationService()
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} initialized", this.GetType()));
		}

		#region Properties
		private CABasicAnimation rotationAnimation = new CABasicAnimation()
		{
			KeyPath = "transform.rotation.z",
			To = new NSNumber(Math.PI * 2),
			Duration = 0.6,
			Cumulative = true,
			RepeatCount = float.MaxValue
		};

		#endregion

		#region Public methods
		public void StartRotation(UIView view)
		{
			view.Layer.AddAnimation(rotationAnimation, rotationAnimationKey);
		}

		public void CompleteRotation(UIView view)
		{
			view.Layer.RemoveAnimation(rotationAnimationKey);
		}

		public void ScaleEffect(UIView view)
		{
			UIView.AnimateNotify(0.1, () => { view.Transform = CGAffineTransform.MakeScale(1.2f, 1.2f); },
			                     (bool finished) => { view.Transform = CGAffineTransform.MakeIdentity(); });
		}

		public void Dispose()
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
		}
		#endregion
	}
}
