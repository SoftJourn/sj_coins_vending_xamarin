namespace Softjourn.SJCoins.Core.Common
{
    public static class Constant
    {
        public const string NavigationKey = "NavigationParams";
        public const string BottomSheetFragmentTag = "PREVIEW_TAG";

        //URLs
        public const string BaseUrl = "https://vending.softjourn.if.ua";
        public const string VendingApiVersion = "/v1";
        public const string CoinsApiVersion = "/v1";
        public const string UrlAuthService = "/api/auth";
        public const string UrlVendingService = "/api/vending" + VendingApiVersion;
        public const string UrlCoinService = "/api/coins" + CoinsApiVersion;

        //URL for testing server
        //public const string BaseUrl = "https://sjcoins-testing.softjourn.if.ua";
        //public const string VendingApiVersion = "/v1";
        //public const string CoinsApiVersion = "/v1";
        //public const string UrlAuthService = "/auth";
        //public const string UrlVendingService = "/vending" + VendingApiVersion;
        //public const string UrlCoinService = "/coins" + CoinsApiVersion;
        //public const string UrlLogin = UrlAuthService + "/oauth/token";

        //Recycler View Types
        public const string DefaultRecyclerView = "DEFAULT";

        //See All button Tags
        public const string Favorites = "Favorites";

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
        public const string ProfileOptionsShareFundsIconName = "ic_share_funs";
        public const string ProfileOptionsSelectMachineIconName = "ic_select_machine";
        public const string ProfileOptionsLogoutIconName = "ic_logout";

        public static class HttpHeader
        {
            public const string AuthorizationKey = "Authorization";
            public const string ContentTypeKey = "Content-Type";

            public const string BasicAuthorizationValue = "Basic dXNlcl9jcmVkOnN1cGVyc2VjcmV0";
            public const string ApplicationFormValue = "application/x-www-form-urlencoded";
            public const string ApplicationJsonValue = "application/json";
            public const string MultipartFormDataValue = "multipart/form-data";
            public const string FormDataValue = "form-data";
        }

        public static class HttpParameter
        {
            public const string PasswordKey = "password";
            public const string GrantTypeKey = "grant_type";
            public const string UserNameKey = "username";
            public const string TokenValueKey = "token_value";

            public const string PasswordValue = "password";
            public const string RefreshTokenValue = "refresh_token";
        }
    }
}