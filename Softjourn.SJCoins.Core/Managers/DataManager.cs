using System.Collections.Generic;
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
            product.IsProductFavorite = true;
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
        #endregion
    }
}
