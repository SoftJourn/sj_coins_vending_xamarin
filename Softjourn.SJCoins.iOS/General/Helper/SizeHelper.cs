using UIKit;
using System;
using CoreGraphics;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
    public static class SizeHelper
    {
        #region Constants
        private static float phoneWidthCoefficient = 3.66f;           // Home page horizontal cell width coeficient. (Collection)
        private static float padWidthCoefficient = 7.1f;              // Home page horizontal cell width coeficient. (Collection)

		private static float CategoryNameLabelHeight = 44.0f;         
		private static float BottomRetreat = 12.0f;

        private static nfloat calculatedHeight = 0;
        #endregion

        #region Properties
        private static readonly UIUserInterfaceIdiom idiom = UIDevice.CurrentDevice.UserInterfaceIdiom;
        public static readonly CGRect mainBounds = UIScreen.MainScreen.Bounds;
		#endregion

		#region Public methods
		public static nfloat VerticalCellHeight()
		{
            if (calculatedHeight == 0)
            {
				var productCellWidth = idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(mainBounds.Width / padWidthCoefficient) : RoundingOf(mainBounds.Width / phoneWidthCoefficient);
				calculatedHeight = CategoryNameLabelHeight + productCellWidth + Const.PhoneNameLabelRetreat + Const.MaxPhoneNameLabelHeight + Const.MaxPhonePriceLabelHeight + Const.PhonePriceLabelRetreat + BottomRetreat;
			}
			return calculatedHeight;
		}

        public static nfloat HorizontalCellWidth()
        {
            return idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(mainBounds.Width / padWidthCoefficient) : RoundingOf(mainBounds.Width / phoneWidthCoefficient);
        }
        #endregion

        #region Private methods
        private static nfloat RoundingOf(nfloat digit)
        {
            return (nfloat)Math.Round((Decimal)digit, 0, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
