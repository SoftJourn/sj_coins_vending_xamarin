namespace TrololoWorld.utils
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
        public const string BaseUrl = "https://vending.softjourn.if.ua/api";
        //URL for testing server
        //public const string BASE_URL = "https://sjcoins-testing.softjourn.if.ua";

        //Test URL
        //public const string BASE_URL = "http://192.168.102.251:8111";

        public const string VendingApiVersion = "v1/";
        public const string CoinsApiVersion = "api/v1/";

        public const string UrlAuthService = BaseUrl + "/auth/";
        public const string UrlVendingService = BaseUrl + "/vending/" + VendingApiVersion;
        public const string UrlCoinService = BaseUrl + "/coins/" + CoinsApiVersion;

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
    }
}