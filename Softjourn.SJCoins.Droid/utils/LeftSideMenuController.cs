using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Products;

namespace Softjourn.SJCoins.Droid.Utils
{
    public class LeftSideMenuController
    {
        private NavigationView _navigationView;

        public LeftSideMenuController(NavigationView navigationView)
        {
            this._navigationView = navigationView;
        }

        /**
         * Method is using to populate Left Side menu with needed items
         *
         * @param menu           to be worked with
         * @param categoriesList List of categories derived from server
         */
        public void AddCategoriesToMenu(IMenu menu, List<Categories> categoriesList)
        {

            var menuItem = menu.FindItem(Resource.Id.categories_subheader);
            var subMenu = menuItem.SubMenu;
            subMenu.Clear();

            foreach (var currentCategory in categoriesList)
            {
                subMenu.Add(currentCategory.Name);
            }

            for (int i = 4; i < menu.Size(); i++)
            {
                var item = menu.GetItem(i);
                item.SetCheckable(true);
            }
        }

        /**
         * Makes all items in NavBar not checked
         * before setting chosen item as checked
         */
        public void UnCheckAllMenuItems(NavigationView navigationView)
        {
            var menu = navigationView.Menu;
            for (var i = 0; i < menu.Size(); i++)
            {
                var item = menu.GetItem(i);
                if (item.HasSubMenu)
                {
                    var subMenu = item.SubMenu;
                    for (var j = 0; j < subMenu.Size(); j++)
                    {
                        var subMenuItem = subMenu.GetItem(j);
                        subMenuItem.SetChecked(false);
                    }
                }
                else
                {
                    item.SetChecked(false);
                }
            }
        }

        /**
         * Is using to find item of the category in categories list on the LeftSideMenu.
         * Category could be derived from server on any position of the list so there is no
         * known position of exact category
         *
         * @param category name of the category to be found in the LeftSideMenu
         * @return position of item in Left Side Menu from given name of the category
         */
        public int GetItemPosition(string category)
        {
            var position = 0;
            for (var i = 0; i < _navigationView.Menu.Size(); i++)
            {
                if (_navigationView.Menu.GetItem(i).TitleFormatted.ToString().Equals(category))
                {
                    position = i;
                }
            }
            return position;
        }

        /**
         * Method to set checked correct category in LeftSide Menu if it was chosen from MainMenu
         *
         * @param category category name derived from Fragment
         */
        public void SetCheckedCategory(string category)
        {
            switch (category)
            {
                //case Const.ALL_ITEMS:
                //    mNavigationView.getMenu().getItem(0).setChecked(true);
                //    break;
                //case Const.FAVORITES:
                //    mNavigationView.getMenu().getItem(1).setChecked(true);
                //    break;
                //default:
                //    break;
            }
        }
    }
}