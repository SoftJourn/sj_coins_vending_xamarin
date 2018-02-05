using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Sources;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.HomePage
{
    [Register("HomeViewController")]
    public partial class HomeViewController : BaseViewController<HomePresenter>, IHomeView, IUISearchControllerDelegate, IUISearchBarDelegate, IDisposable
    {
        #region Properties
        public List<Categories> Categories { get; private set; }

        private List<Categories> MatchesCategory { get; set; }
        private bool pullToRefreshTrigged = false;
        private string currentBalance = "";
        private string currentUser = "";
        private HomeViewSource tableSource = new HomeViewSource();
        private UISearchController searchController;
        private UITapGestureRecognizer accountTapGesture;
        #endregion

        #region Constructor
        public HomeViewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Controller Life cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigurePage();
            ConfigureAvatarImage(AvatarImage);
            ConfigureTableView();
            CustomizeUIDependingOnVersion();
            Presenter.OnStartLoadingPage();
            NavigationController.SetNavigationBarHidden(true, false);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            RefreshFavoritesCell();
            Presenter.UpdateBalanceView();
            Presenter.GetImageFromServer();
        }
        #endregion

        #region BaseViewController
        public override void AttachEvents()
        {
            base.AttachEvents();
            AccountView.AddGestureRecognizer(accountTapGesture);
            SearchButton.Clicked += OnSearchClicked;
            tableSource.HomeViewSource_ItemSelected += OnItemSelected;
            tableSource.HomeViewSource_SeeAllClicked += OnSeeAllClicked;
            tableSource.HomeViewSource_BuyExecuted += OnBuyActionClicked;
            tableSource.HomeViewSource_AddDeleteFavoriteExecuted += OnFavoriteActionClicked;
        }

        public override void DetachEvents()
        {
            AccountView.RemoveGestureRecognizer(accountTapGesture);
            SearchButton.Clicked -= OnSearchClicked;
            tableSource.HomeViewSource_ItemSelected -= OnItemSelected;
            tableSource.HomeViewSource_SeeAllClicked -= OnSeeAllClicked;
            tableSource.HomeViewSource_BuyExecuted -= OnBuyActionClicked;
            tableSource.HomeViewSource_AddDeleteFavoriteExecuted -= OnFavoriteActionClicked;
            base.DetachEvents();
        }

        public override void ShowProgress(string message)
        {
            if (!pullToRefreshTrigged)
                base.ShowProgress(message);
        }

        public override void HideProgress()
        {
            if (!pullToRefreshTrigged)
                base.HideProgress();

            pullToRefreshTrigged = false;
        }
        #endregion

        #region IHomeView implementation
        public void SetAccountInfo(Account account)
        {
            // Show user balance on start
            currentUser = account.Name + " " + account.Surname;
            var balance = account.Amount.ToString();
            currentBalance = balance;
            SetBalance(balance, currentUser);
        }

        public void SetUserBalance(string balance)
        {
            // Show user balance after buying
            SetBalance(balance, currentUser);
        }

        public void SetMachineName(string name)
        {
            // Set chosenMachine name as title
            MachineNameLabel.Text = name;
        }

        public void ShowProducts(List<Categories> listCategories)
        {
            NoItemsLabel.Hidden = true;
            Categories = listCategories;
            tableSource.Categories = Categories;
            TableView.ReloadData();
            NavigationController.SetNavigationBarHidden(false, false);

            if (searchController == null)
                ConfigureSearch();
            
            ShowScreenAnimated(true);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                NavigationItem.SearchController = searchController;
            else
                SearchButton.Enabled = true;
        }

        public void ImageAcquired(byte[] receipt)
        {
            // Method trigged when data taken from server or dataManager
            var image = UIImage.LoadFromData(NSData.FromArray(receipt));
            AvatarImage.Image = image;
        }

        public void ServiceNotAvailable()
        {
            // Set chosenMachine name as title
            NoItemsLabel.Hidden = false;
            NoItemsLabel.Text = "Service is not available.";
            ShowScreenAnimated(false);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                NavigationItem.SearchController = null;
            else
                SearchButton.Enabled = false;
        }

        public void LastUnavailableFavoriteRemoved(Product product)
        {
            RefreshFavoritesCell();
        }

        public void FavoriteChanged(Product product)
        {
            RefreshFavoritesCell();
        }
        #endregion

        #region Private methods
        private void ConfigurePage()
        {
            //Hide no items label
            NoItemsLabel.Hidden = true;
            AccountView.Alpha = 0.0f;
            TableView.Alpha = 0.0f;

            accountTapGesture = new UITapGestureRecognizer(AccountTap)
            {
                Enabled = true
            };
            AccountView.AddGestureRecognizer(accountTapGesture);

            NavigationController.NavigationBar.TintColor = UIColorConstants.MainGreenColor;

            NSUserDefaults.StandardUserDefaults.SetBool(true, Const.FIRSTLOGIN);
        }

        private void ConfigureTableView()
        {
            // Configure datasource and delegate
            TableView.Source = tableSource;
            TableView.AlwaysBounceVertical = true;
            TableView.ScrollsToTop = true;
        }

        private void ConfigureSearch()
        {
            searchController = new UISearchController(searchResultsController: null)
            {
                WeakDelegate = this,
                DimsBackgroundDuringPresentation = false,
            };
            searchController.SearchBar.Delegate = this;
            DefinesPresentationContext = false;

            MatchesCategory = new List<Categories>
            {
                new Categories { Name = "Matches", Products = new List<Product>() }
            };
        }

        private void ConfigureAvatarImage(UIImageView imageView)
        {
            // Make image rounded
            CALayer imageCircle = imageView.Layer;
            imageCircle.CornerRadius = AvatarImage.Frame.Height / 2;
            imageCircle.MasksToBounds = true;
            AvatarImage.Image = UIImage.FromBundle("NoAvatarSmall.png");
        }

        private void CustomizeUIDependingOnVersion() 
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                // Code that uses features from iOS 11.0 and later
                NavigationItem.SearchController = searchController;
                NavigationItem.HidesSearchBarWhenScrolling = true;
                // Disable SearchButton
                NavigationItem.RightBarButtonItem = null;
            }
            else
            {
                // Code to support earlier iOS versions
            }

            UISearchBar.Appearance.TintColor = UIColorConstants.MainGreenColor;
        }

        private void SetBalance(string balance, string user)
        {
            MyBalanceLabel.Text = user + ": " + balance;
        }

        private void RefreshFavoritesCell()
        {
            // TODO refactoring 
            if (Categories != null)
            {
                var newList = Presenter.GetCategoriesList();
                tableSource.Categories = newList;
                ReloadTable();
            }
        }

        private void DismissSearchAndEnableRefresh()
        {
            if (searchController != null && searchController.Active)
            {
                TableView.Bounces = true;
                searchController.DismissViewController(true, null);
                searchController.SearchBar.Text = "";
            }
        }

        private void ReloadTable()
        {
            TableView.ReloadSections(new NSIndexSet(0), UITableViewRowAnimation.None);
        }

        private void StyleTableForSearchResult() 
        {
            NoItemsLabel.Hidden = false;
            NoItemsLabel.Text = "Product not found.";
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }

        private void DefaultTableStyle()
        {
            NoItemsLabel.Hidden = true;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
        }
        #endregion

        #region Event handlers
        private void OnItemSelected(object sender, Product product)
        {
            // Trigg presenter that user click on some product for showing details controllers
            DismissSearchAndEnableRefresh();
            Presenter.OnProductDetailsClick(product.Id);
        }

        private void OnSeeAllClicked(object sender, string categoryName)
        {
            // Trigg presenter that user click on SeeAll button
            DismissSearchAndEnableRefresh();
            Presenter.OnShowAllClick(categoryName);
        }

        private void OnBuyActionClicked(object sender, Product product)
        {
            // Trigg presenter that user click Buy action on preview page 
            Presenter.OnBuyProductClick(product);
        }

        private void OnFavoriteActionClicked(object sender, Product product)
        {
            // Trigg presenter that user click Favorite action on preview page 
            Presenter.OnFavoriteClick(product);
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            // Handle clicking on the Search button
            searchController.SearchBar.Text = "";
            PresentViewController(searchController, true, null);

            // Disable PullToRefresh
            TableView.Bounces = false;
        }

        private void AccountTap(UITapGestureRecognizer gestureRecognizer)
        {
            // Trigg presenter that user click on account
            Presenter.OnProfileButtonClicked();
        }
        #endregion

        #region Throw TableView to parent
        protected override UIScrollView GetRefreshableScrollView() => TableView;

        protected override void PullToRefreshTriggered(object sender, EventArgs e)
        {
            StopRefreshing();
            pullToRefreshTrigged = true;
            Presenter.OnStartLoadingPage();
        }
        #endregion

        protected override void ShowAnimated(bool loadSuccess)
        {
            AccountView.Alpha = loadSuccess ? 1.0f : 0f;
            TableView.Alpha = 1.0f;
            SearchButton.Enabled = loadSuccess;
            SearchButton.TintColor = UIColorConstants.MainGreenColor;
        }

        #region IUISearchControllerDelegate
        [Export("willDismissSearchController:")]
        public void WillDismissSearchController(UISearchController searchController)
        {
            // Show all categories.
            tableSource.Categories = Categories;
            TableView.ReloadData();
            DismissSearchAndEnableRefresh();
            DefaultTableStyle();
        }

        [Export("didPresentSearchController:")]
        public void DidPresentSearchController(UISearchController searchController)
        {
            // Disable PullToRefresh
            TableView.Bounces = false;
        }
        #endregion

        #region IUISearchBarDelegate
        [Export("searchBarSearchButtonClicked:")]
        public void SearchButtonClicked(UISearchBar searchBar)
        {
            searchBar.ResignFirstResponder();
        }

        [Export("searchBar:textDidChange:")]
        public void TextChanged(UISearchBar searchBar, string searchText)
        {
            var matchCategory = MatchesCategory[0];
            matchCategory.Products.Clear();

            if (searchController.Active && searchText != "")
            {
                var search = searchController.SearchBar.Text.Trim();
                foreach (var category in Categories)
                {
                    if (category.Name == Const.FavoritesCategory)
                        continue;

                    var products = category.Products.FindAll(item => item.Name.ToLower().Contains(search.ToLower()));
                    matchCategory.Products.AddRange(products);
                }

                if (matchCategory.Products.Count > 0)
                {
                    matchCategory.Products = matchCategory.Products.Distinct().ToList();
                    matchCategory.Name = "Matches";
                    DefaultTableStyle();
                }
                else 
                {
                    matchCategory.Name = "";
                    StyleTableForSearchResult();
                }
                MatchesCategory[0] = matchCategory;
                tableSource.Categories = MatchesCategory;
            }
            else 
            {
                DefaultTableStyle();
                tableSource.Categories = Categories;
            }

            TableView.ReloadData();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
            base.Dispose(disposing);
        }
    }
}
