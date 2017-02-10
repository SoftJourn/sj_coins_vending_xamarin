
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class DetailsPagerAdapter : PagerAdapter
    {
        Context _context;
        private List<string> _images;
        LayoutInflater _layoutInflater;

        public DetailsPagerAdapter(Context context, List<string> images)
        {
            _context = context;
            _images = images;
            _layoutInflater = _context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
        }


        public override int Count => _images.Count;


        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == ((LinearLayout)obj);
        }


        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var itemView = _layoutInflater.Inflate(Resource.Layout.activity_details_view_pager_item, container, false);

            var imageView = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            Picasso.With(_context).Load(_images[position]).Into(imageView);

            imageView.Click += (o, e) =>
                {
                //your code here
            };

            container.AddView(itemView);
            return itemView;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
        }
    }
}