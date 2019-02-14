using System;
using CoreGraphics;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.General.Helper
{
    public static class SizeHelper
    {
        #region Constants

        private const float PhoneWidthCoefficient = 3.66f; // Home page horizontal cell width coefficient. (Collection)
        private const float PadWidthCoefficient = 7.1f; // Home page horizontal cell width coefficient. (Collection)

        private const float CategoryNameLabelHeight = 44.0f;
        private const float BottomRetreat = 12.0f;

        private static nfloat calculatedHeight = 0;

        #endregion

        #region Properties

        private static readonly UIUserInterfaceIdiom Idiom = UIDevice.CurrentDevice.UserInterfaceIdiom;
        public static readonly CGRect MainBounds = UIScreen.MainScreen.Bounds;

        #endregion

        #region Public methods

        public static nfloat VerticalCellHeight()
        {
            if (calculatedHeight == 0)
            {
                var productCellWidth = Idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(MainBounds.Width / PadWidthCoefficient) : RoundingOf(MainBounds.Width / PhoneWidthCoefficient);
                calculatedHeight = CategoryNameLabelHeight + productCellWidth + Const.PhoneNameLabelRetreat + Const.MaxPhoneNameLabelHeight + Const.MaxPhonePriceLabelHeight + Const.PhonePriceLabelRetreat + BottomRetreat;
            }

            return calculatedHeight;
        }

        public static nfloat HorizontalCellWidth() => Idiom == UIUserInterfaceIdiom.Pad
            ? RoundingOf(MainBounds.Width / PadWidthCoefficient)
            : RoundingOf(MainBounds.Width / PhoneWidthCoefficient);

        #endregion

        #region Private methods

        private static nfloat RoundingOf(nfloat digit) =>
            (nfloat) Math.Round((decimal) digit, 0, MidpointRounding.AwayFromZero);

        #endregion
    }
}
