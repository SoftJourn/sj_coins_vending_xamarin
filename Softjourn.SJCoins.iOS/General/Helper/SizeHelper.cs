using UIKit;
using System;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
    public class SizeHelper
    {
        #region Constants
        public const float phoneVerticalCoefficient = 3.0f;         // Home page vertical cell height coeficient. (Table)
        public const float padVerticalCoefficient = 4.6f;           // Home page vertical cell height coeficient. (Table)

        public const float phoneWidthCoefficient = 3.9f;            // Home page horizontal cell width coeficient. (Collection)
        public const float padWidthCoefficient = 7.1f;              // Home page horizontal cell width coeficient. (Collection)

        public const float phoneDetailHeightCoefficient = 0.5f;     // Detail Page Header height. (PageController)
        public const float padDetailHeightCoefficient = 0.8f;       // Detail Page Header height. (PageController)

		public const float CategoryNameLabelHeight = 38.0f;      
		public const float BottomRetreat = 6.0f;      
		#endregion

		#region Properties
		private UIUserInterfaceIdiom idiom = UIDevice.CurrentDevice.UserInterfaceIdiom;
        #endregion

        #region Public methods
        public nfloat VerticalCellHeight(nfloat height, nfloat width)
        {
            return CellHeight(idiom, width);
        }

        public nfloat HorizontalCellWidth(nfloat width)
        {
            return idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(width / padWidthCoefficient) : RoundingOf(width / phoneWidthCoefficient);
        }

        public nfloat DetailHeaderHeight(nfloat height)
        {
            return idiom == UIUserInterfaceIdiom.Pad ? RoundingOf(height * padDetailHeightCoefficient) : RoundingOf(height * phoneDetailHeightCoefficient);
		}
        #endregion

        #region Private methods
        private nfloat RoundingOf(nfloat digit)
        {
            return (nfloat)Math.Round((Decimal)digit, 0, MidpointRounding.AwayFromZero);
        }

        private nfloat CellHeight(UIUserInterfaceIdiom idioma, nfloat width)
        {
            var productCellWidth = idioma == UIUserInterfaceIdiom.Pad ? RoundingOf(width / padWidthCoefficient) : RoundingOf(width / phoneWidthCoefficient);
            var productCellHeight = CategoryNameLabelHeight + productCellWidth + Const.MaxPhoneNameLabelHeight + Const.PhoneNameLabelRetreat + Const.MaxPhonePriceLabelHeight + Const.PhonePriceLabelRetreat + BottomRetreat;
            return productCellHeight;
        }
        #endregion
    }
}
