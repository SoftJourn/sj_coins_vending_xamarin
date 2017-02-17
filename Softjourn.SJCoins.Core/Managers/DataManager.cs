using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Utils;

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
			// If category favorites not exist create it  
			// and insert in ProductList with index 0 (as first element)
			if (ProductList[0].Name != Const.Favorites) {
				var favoriteCategory = new Categories()
				{
					Name = Const.Favorites,
					Products = new List<Product>()
				};
				ProductList.Insert(0, favoriteCategory);
			}

			foreach (var category in ProductList)
			{
				// If category Favorites exist in ProductList add given products to it 
				if (category.Name == Const.Favorites)
				{
					category.Products.Add(product);
					continue;
				}
				else {
					// Change IsProductFavorite flag in this products if they exist in anothere categories
					foreach (var currentProduct in category.Products)
					{
						if (currentProduct.Id == product.Id)
						{
							currentProduct.IsProductFavorite = true;
							break;
						}
					}
				}
			}
		}

        //Remove Product From Favorites in LocalStorage
        public void RemoveProductFromFavorite(Product product)
        {
			foreach (var category in ProductList)
            {
                if (category.Name == Const.Favorites)
				{
					// Remove given product from Favorites category 
					foreach (var currentProduct in category.Products)
					{
						if (currentProduct.Id == product.Id)
						{
							category.Products.Remove(currentProduct);
							break;
						}
					}
                }
                else {
					// Change IsProductFavorite flag in this product if it exist in anothere categories
					foreach (var currentProduct in category.Products)
                    {
                        if (currentProduct.Id == product.Id)
                        {
							currentProduct.IsProductFavorite = false;
                            break;
                        }
                    }
                }
            }

			// If deleted product was last item in list remove Favorites category
			if (ProductList[0].Products.Count == 0)
			{
				// Remove Favorites category
				ProductList.RemoveAt(0);
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
            return new List<Product>();
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
