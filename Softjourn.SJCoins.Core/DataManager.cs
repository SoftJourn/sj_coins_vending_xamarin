using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Models.AccountInfo;
using Softjourn.SJCoins.Core.Models.Products;

namespace Softjourn.SJCoins.Core.Managers
{
    public sealed class DataManager
    {
        /// <summary>
        /// Predefined elements for keys in Dictionary in
        /// nutrition facts should be exact in below order
        /// </summary>
        private readonly List<string> _facts = new List<string>
        {
            "Calories",
            "Fat",
            "Saturates",
            "Protein",
            "Carbohydrates",
            "Sugars",
            "Salt",
            "Fibre"
        };

        public Account Profile { get; set; }

        public List<Categories> ProductList { get; set; }

        public byte[] Avatar { get; set; }

        /// <summary>
        /// AddProduct To Favorite in LocalStorage
        /// </summary>
        /// <param name="product">Product to add</param>
        public void AddProductToFavorite(Product product)
        {
            product.IsProductFavorite = true;
            // If category favorites not exist create it  
            // and insert in ProductList with index 0 (as first element)
            if (ProductList[0].Name != Constant.Favorites)
            {
                var favoriteCategory = new Categories()
                {
                    Name = Constant.Favorites,
                    Products = new List<Product>()
                };
                ProductList.Insert(0, favoriteCategory);
            }

            foreach (var category in ProductList)
            {
                // If category Favorites exist in ProductList add given products to it 
                if (category.Name == Constant.Favorites)
                {
                    category.Products.Add(product);
                    continue;
                }

                // Change IsProductFavorite flag in this products if they exist in another categories
                foreach (var currentProduct in category.Products)
                {
                    if (currentProduct.Id == product.Id)
                    {
                        currentProduct.IsProductFavorite = true;
                        currentProduct.IsProductInCurrentMachine = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Remove Product From Favorites in LocalStorage
        /// </summary>
        /// <param name="product">Product to remove</param>
        public void RemoveProductFromFavorite(Product product)
        {
            foreach (var category in ProductList)
            {
                if (category.Name == Constant.Favorites)
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
                else
                {
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

        /// <summary>
        /// Adds Or remove Product From Favorite
        /// </summary>
        /// <param name="product">Product whcih favorite status shouls be changed</param>
        /// <returns>Input Product but with changed favorite status</returns>
        public Product ChangeProductsFavoriteStatus(Product product)
        {
            if (product.IsProductFavorite)
            {
                RemoveProductFromFavorite(product);
            }
            else
            {
                AddProductToFavorite(product);
            }

            return GetProductFromListById(product.Id);
        }

        /// <summary>
        /// Find product in product list by product's id
        /// </summary>
        /// <param name="productId">Id of product</param>
        /// <returns>First founded Product with given ID</returns>
        public Product GetProductFromListById(int productId)
        {
            return ProductList.SelectMany(category => category.Products).FirstOrDefault(product => product.Id == productId);
        }

        /// <summary>
        /// Returns List of Products
        /// </summary>
        /// <param name="categoryInput">category name to be found</param>
        /// <returns>Founded List of products by given category name</returns>
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

        /// <summary>
        /// Sort Products by Name in given category Ascending or Descending
        /// </summary>
        /// <param name="category">given category name</param>
        /// <param name="sortingForward">Ascending if true, Descending if false</param>
        /// <returns>Sorted Products by Name List of given category</returns>
        public List<Product> GetSortedByNameProductsList(string category, bool sortingForward)
        {
            var productList = GetProductListByGivenCategory(category);

            return sortingForward
                ? productList.OrderBy(product => product.Name).ToList()
                : productList.OrderByDescending(product => product.Name).ToList();
        }

        /// <summary>
        /// Sort Products by Price in given category Ascending or Descending
        /// </summary>
        /// <param name="category">given category name</param>
        /// <param name="sortingForward">Ascending if true, Descending if false</param>
        /// <returns>Sorted Products by Price List of given category</returns>
        public List<Product> GetSortedByPriceProductsList(string category, bool sortingForward)
        {
            var productList = GetProductListByGivenCategory(category);

            return sortingForward
                ? productList.OrderBy(product => product.IntPrice).ToList()
                : productList.OrderByDescending(product => product.IntPrice).ToList();
        }

        /// <summary>
        /// Set Nutrition facts for each product in ProductList in correct order
        /// </summary>
        public void SetNutritionFactsInCorrectOrder()
        {
            foreach (var category in ProductList)
            {
                foreach (var product in category.Products)
                {
                    var newSort = SortNutritionFacts(product.NutritionFacts);
                    product.NutritionFacts = newSort;
                }
            }
        }

        /// <summary>
        /// Change NutritionFacts Dicitionary to have elements in correct order
        /// Correct Order is set in _facts var.
        /// If there is no needed fact in Dictionary then fact not adds to
        /// result dictionary.
        /// All not hardcoded facts adds to the result dictionary at the end
        /// after all hardcoded facts.
        /// </summary>
        /// <param name="inDictionary">Input Dictionary of Nutrition Facts</param>
        /// <returns>Returns Sorted Converted Dictionary</returns>
        private Dictionary<string, string> SortNutritionFacts(Dictionary<string, string> inDictionary)
        {
            var result = new Dictionary<string, string>();

            foreach (var fact in _facts)
            {
                if (inDictionary.ContainsKey(fact))
                {
                    result.Add(fact, inDictionary[fact]);
                }
            }
            foreach (var fact in inDictionary.Keys)
            {
                if (!_facts.Contains(fact))
                {
                    result.Add(fact, inDictionary[fact]);
                }
            }

            return result;
        }
    }
}
