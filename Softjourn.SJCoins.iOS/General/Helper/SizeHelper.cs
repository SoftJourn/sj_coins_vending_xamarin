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
		public const float padWidthCoefficient = 7.9f;              // Home page horizontal cell width coeficient. (Collection)
		
		public const float phoneDetailHeightCoefficient = 0.5f;     // Detail Page Header height. (PageController)
		public const float padDetailHeightCoefficient = 0.8f;       // Detail Page Header height. (PageController)
		#endregion

		#region Properties
		private UIUserInterfaceIdiom idiom = UIDevice.CurrentDevice.UserInterfaceIdiom;
		#endregion

		#region Public methods
		public nfloat VerticalCellHeight(nfloat height)
        {
            return idiom == UIUserInterfaceIdiom.Pad ? height / padVerticalCoefficient : height / phoneVerticalCoefficient;
        }

        public nfloat HorizontalCellWidth(nfloat width)
		{
			return idiom == UIUserInterfaceIdiom.Pad ? width / padWidthCoefficient : width / phoneWidthCoefficient;
		}

		public nfloat DetailHeaderHeight(nfloat height)
		{
            return idiom == UIUserInterfaceIdiom.Pad ? height * padDetailHeightCoefficient : height * phoneDetailHeightCoefficient;
		}
		#endregion
	}
}
