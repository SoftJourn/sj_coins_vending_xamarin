using System;
using Android.Support.V7.Widget;

namespace Softjourn.SJCoins.Droid.UI.Listeners
{
    //Class for checking if scrolled list is in its 
    //bottom position 
    //is used for continuous loading
    public class XamarinRecyclerViewOnScrollListener : RecyclerView.OnScrollListener
    {
        public delegate void LoadMoreEventHandler(object sender, EventArgs e);
        public event LoadMoreEventHandler LoadMoreEvent;

        private readonly LinearLayoutManager LayoutManager;

        public XamarinRecyclerViewOnScrollListener(LinearLayoutManager layoutManager)
        {
            LayoutManager = layoutManager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var pastVisibleItems = LayoutManager.FindFirstVisibleItemPosition();

            if (visibleItemCount + pastVisibleItems >= totalItemCount)
                LoadMoreEvent(this, null);
        }
    }
}