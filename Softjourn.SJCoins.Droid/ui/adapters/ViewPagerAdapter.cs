using Android.Content;
using Android.Support.V4.View;
using Android.Views;

namespace Softjourn.SJCoins.Droid.ui.adapters
{
    public class ViewPagerAdapter : PagerAdapter
    {
        private LayoutInflater _layoutInflater;
        private readonly int[] _layouts;
        private readonly Context _context;

        public override int Count => _layouts.Length;

        public ViewPagerAdapter(int[] layouts, Context context)
        {
            _layouts = layouts;
            _context = context;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            _layoutInflater = _context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;

            var view = _layoutInflater.Inflate(_layouts[position], container, false);
            container.AddView(view);

            return view;
        }

        public override bool IsViewFromObject (View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object _object)
        {
            var view = (View)_object;
            container.RemoveView(view);
        }
    }
}