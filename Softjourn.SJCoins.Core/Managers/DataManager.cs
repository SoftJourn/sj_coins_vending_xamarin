using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;

namespace Softjourn.SJCoins.Core.Managers
{
    public class DataManager
    {
        #region Properties
        public Account Profile { get; set; }
        public List<Categories> ProductList { get; set; }
        #endregion

        #region Methods

        //AddProduct To Favorite in LocalStorage
        public void AddProductToFavorite(Product product)
        {
            product.IsProductFavorite = true;
            foreach (var category in ProductList)
            {
                if (category.Name == "Favorites")
                {
                    category.Products.Add(product);
                }
                else
                {
                    foreach (var currentProduct in category.Products)
                    {
                        if (currentProduct.Id == product.Id)
                        {
                            product = currentProduct;
                            break;
                        }
                    }
                }
            }
        }

        //Remove Product From Favorites in LocalStorage
        public void RemoveProductFromFavorite(Product product)
        {
            product.IsProductFavorite = false;
            foreach (var category in ProductList)
            {
                if (category.Name == "Favorites")
                {
                    foreach (var currentProduct in category.Products)
                    {
                        if (currentProduct.Id == product.Id)
                        {
                            category.Products.Remove(currentProduct);
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var currentProduct in category.Products)
                    {
                        if (currentProduct.Id == product.Id)
                        {
                            product = currentProduct;
                            break;
                        }
                    }
                }
            }
        }

        public Product GetProductFromListById(int productId)
        {
            var categoriesList = ProductList;

            foreach (var category in categoriesList)
            {
                foreach (var product in category.Products)
                {
                    if (product.Id == productId)
                    {
                        return product;
                    }
                }
            }
            return null;
        }

        public List<Product> GetProductListByGivenCategory(string categoryInput)
        {
            foreach (var category in ProductList)
            {
                if (categoryInput == category.Name)
                {
                    return category.Products;
                }
            }
            return null;
        }

        public List<Product> GetSortedByNameProductsList(string category, bool sortingForward)
        {
            var productList = GetProductListByGivenCategory(category);
            return sortingForward ? productList.OrderBy(product => product.Name).ToList() : productList.OrderByDescending(product => product.Name).ToList();
        }

        public List<Product> GetSortedByPriceProductsList(string category, bool sortingForward)
        {
            var productList = GetProductListByGivenCategory(category);
            return sortingForward ? productList.OrderBy(product => product.IntPrice).ToList() : productList.OrderByDescending(product => product.IntPrice).ToList();
        }
        #endregion
    }
}
