namespace Softjourn.SJCoins.Core.Utils
{ 
    public class Const
    {
        public const string SjCoinsPreferences = "SJ_COINS_PREFERENCES";
        public const string AccessToken = "ACCESS_TOKEN";
        public const string RefreshToken = "REFRESH_TOKEN";
        public const string ExpirationDate = "EXPIRATION_DATE";

        public const string SelectedMachineName = "SELECTED_MACHINE_NAME";
        public const string SelectedMachineId = "SELECTED_MACHINE_ID";

        public const string UserNamePreferencesKey = "USER_NAME_PREFERENCES_KEY";
        public const string UserBalancePreferencesKey = "USER_BALANCE_PREFERENCES_KEY";

        //Fragments tags
        public const string TagProductsLastAddedFragment = "TAG_PRODUCTS_LAST_ADDED_FRAGMENT";
        public const string TagProductsBestSellersFragment = "TAG_PRODUCTS_BEST_SELLERS_FRAGMENT";
        public const string TagFavoritesFragment = "TAG_FAVORITES_FRAGMENT";
        public const string TagAllProductsFragment = "TAG_ALL_PRODUCTS_FRAGMENT";

        //URLs
        public const string BaseUrl = "https://vending.softjourn.if.ua";
        public const string VendingApiVersion = "v1/";
        public const string CoinsApiVersion = "v1/";
        public const string UrlAuthService = "/api/auth/";
        public const string UrlVendingService = "/api/vending/" + VendingApiVersion;
        public const string UrlCoinService = "/api/coins/" + CoinsApiVersion;
        public const string UrlLogin = UrlAuthService + "oauth/token";

        //URL for testing server
        //public const string BaseUrl = "https://sjcoins-testing.softjourn.if.ua";
        //public const string VendingApiVersion = "v1/";
        //public const string CoinsApiVersion = "v1/";
        //public const string UrlAuthService = "/auth/";
        //public const string UrlVendingService = "/vending/" + VendingApiVersion;
        //public const string UrlCoinService = "/coins/" + CoinsApiVersion;
        //public const string UrlLogin = UrlAuthService + "oauth/token";

        public const string GrantTypePassword = "password";
        public const string GrantTypeRefreshToken = "refresh_token";

        public const bool CallFailed = false;
        public const bool CallSucceed = true;

        //Recycler View Types
        public const string DefaultRecyclerView = "DEFAULT";
        public const string SeeAllSnacksDrinksRecyclerView = "SEE_ALL_SNACKS_DRINKS";

        //See All button Tags
        //are using for correct appearance of SeeAllActivity Label
        public const string LastAdded = "Last Added";
        public const string Favorites = "Favorites";
        public const string BestSellers = "Best Sellers";
        public const string AllItems = "All Items";

        //Headers for HTTP
        public const string HeaderAuthorizationKey = "Authorization";
        public const string HeaderAuthorizationValue = "Basic dXNlcl9jcmVkOnN1cGVyc2VjcmV0";

        public const string HeaderContentTypeKey = "Content-Type";
        public const string HeaderContentTypeValue = "application/x-www-form-urlencoded";

        public const string ActionAddFavorite = "ADD";
        public const string ActionRemoveFavorite = "REMOVE";

        public const string IsFirstTimeLaunch = "IS_FIRST_TIME_LAUNCH";

        public const string ExtrasCategory = "CATEGORY";

        public const int INVALID_PRODUCT_ID = -1;

        public const string ProfileOptionsPurchase = "Purchase";
        public const string ProfileOptionsReports = "Reports";
        public const string ProfileOptionsPrivacyTerms = "Privace and Terms";
        public const string ProfileOptionsHelp = "Help";
        public const string ProfileOptionsShareFunds = "Share Funds";
        public const string ProfileOptionsSelectMachine = "Select Machine";
        public const string ProfileOptionsLogout = "Logout";

        public const string QrScreenScanningTag = "Scan";
        public const string QrScreenGeneratingTag = "Generate";
        public const string ProfileOptionsPurchaseIconName = "ic_purchase";
        public const string ProfileOptionsReportsIconName = "ic_reports";
        public const string ProfileOptionsPrivacyTermsIconName = "ic_privacy_and_terms";
        public const string ProfileOptionsHelpIconName = "ic_help";
        public const string ProfileOptionsShareFundsIconName = "ic_share_funs";
        public const string ProfileOptionsSelectMachineIconName = "ic_select_machine";
        public const string ProfileOptionsLogoutIconName = "ic_logout";
    }
}