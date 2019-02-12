using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Services
{
    public class AnimationService : IDisposable
    {
        private const string RotationAnimationKey = "rotationAnimation";

        public AnimationService()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} initialized", this.GetType()));
        }

        private readonly CABasicAnimation rotationAnimation = new CABasicAnimation
        {
            KeyPath = "transform.rotation.z",
            To = new NSNumber(Math.PI * 2),
            Duration = 0.6,
            Cumulative = true,
            RepeatCount = float.MaxValue
        };

        public void StartRotation(UIView view)
        {
            view.Layer.AddAnimation(rotationAnimation, RotationAnimationKey);
        }

        public void CompleteRotation(UIView view)
        {
            view.Layer.RemoveAnimation(RotationAnimationKey);
        }

        public void ScaleEffect(UIView view)
        {
            UIView.AnimateNotify(0.1, () => { view.Transform = CGAffineTransform.MakeScale(1.2f, 1.2f); },
                                 (bool finished) => { view.Transform = CGAffineTransform.MakeIdentity(); });
        }

        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} disposed", this.GetType()));
        }
    }
}
