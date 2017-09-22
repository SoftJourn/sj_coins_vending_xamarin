using UIKit;
using System;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
    public static class SizeHelper
    {
        #region Constants
        private static float phoneVerticalCoefficient = 3.0f;         // Home page vertical cell height coeficient. (Table)
        private static float padVerticalCoefficient = 4.6f;           // Home page vertical cell height coeficient. (Table)

        private static float phoneWidthCoefficient = 3.66f;            // Home page horizontal cell width coeficient. (Collection)
        private static float padWidthCoefficient = 7.1f;              // Home page horizontal cell width coeficient. (Collection)

        private static float phoneDetailHeightCoefficient = 0.5f;     // Detail Page Header height. (PageController)
        private static float padDetailHeightCoefficient = 0.8f;       // Detail Page Header height. (PageController)

		private static float phoneAccountHeight = 350.0f;             // Account Page Header height.
		private static float padAccountHeight = 400.0f;

		private static float CategoryNameLabelHeight = 38.0f;      
		private static float BottomRetreat = 6.0f;
        #endregion

        #region Properties
        private static readonly UIUserInterfaceIdiom idiom = UIDevice.CurrentDevice.UserInterfaceIdiom;
        private static readonly nfloat width = UIScreen.MainScreen.Bounds.Width;
		#endregion

		#region Public methods
		public static nfloat VerticalCellHeight()
		{
			var productCellWidth = idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(width / padWidthCoefficient) : RoundingOf(width / phoneWidthCoefficient);
			var productCellHeight = CategoryNameLabelHeight + productCellWidth + Const.MaxPhoneNameLabelHeight + Const.PhoneNameLabelRetreat + Const.MaxPhonePriceLabelHeight + Const.PhonePriceLabelRetreat + BottomRetreat;
			return productCellHeight;
		}

        public static nfloat HorizontalCellWidth()
        {
            return idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(width / padWidthCoefficient) : RoundingOf(width / phoneWidthCoefficient);
        }

        public static nfloat DetailHeaderHeight(nfloat height)
        {
            return idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(height * padDetailHeightCoefficient) : RoundingOf(height * phoneDetailHeightCoefficient);
		}

		public static nfloat AccountHeaderHeight()
		{
            return idiom == UIUserInterfaceIdiom.Pad ? padAccountHeight : phoneAccountHeight;
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
