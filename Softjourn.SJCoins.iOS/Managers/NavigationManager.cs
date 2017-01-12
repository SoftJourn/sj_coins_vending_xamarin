using System;
using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.iOS.UI.Controllers.Informative;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class NavigationManager
	{
		public NavigationManager()
		{
		}

		public List<ContentViewController> CreateInformativePages(List<string> titles)
		{
			var pages = new List<ContentViewController>();
			for (int i = 0; i < 4; i++)
			{
				ContentViewController content = Instantiate("Login", "ContentViewController") as ContentViewController;
				content.Index = i;
				content.Title = titles.ElementAt(i);
				pages.Add(content);
			}
			return pages;
		}

		public UIViewController Instantiate(string storyboard, string viewcontroller)
		{
			return UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);
		}
	}
}
