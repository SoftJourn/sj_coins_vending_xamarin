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
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object initialized", this.GetType()));
		}

		#region Properties
		private CABasicAnimation rotationAnimation = new CABasicAnimation()
		{
			KeyPath = "transform.rotation.z",
			To = new NSNumber(Math.PI * 2),
			Duration = 1,
			Cumulative = true,
			RepeatCount = float.MaxValue,
			RepeatDuration = 600               
		};

		private CABasicAnimation scaleAnimation = new CABasicAnimation()
		{
			KeyPath = "scale",
			To = new NSNumber(Math.PI * 2),
			Cumulative = true,
			RepeatCount = float.MaxValue,
			RepeatDuration = 600
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

		public void StartScaling(UIView view)
		{
			UIView.Animate(0.1, 0, UIViewAnimationOptions.CurveLinear | UIViewAnimationOptions.Autoreverse,
						   () => { view.Transform = CGAffineTransform.MakeScale(1.2f, 1.2f); },
			               () => { view.Transform = CGAffineTransform.MakeIdentity(); }
						  );
		}

		public void Dispose()
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
		}
		#endregion
	}
}
