
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class DetailsPagerAdapter : PagerAdapter
    {
        Context _context;
        int[] _images;
        LayoutInflater _layoutInflater;

        public DetailsPagerAdapter(Context context, int[] images)
        {
            _context = context;
            _images = images;
            _layoutInflater = _context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
        }


        public override int Count => _images.Length;


        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == ((LinearLayout)obj);
        }


        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var itemView = _layoutInflater.Inflate(Resource.Layout.activity_details_view_pager_item, container, false);

            var imageView = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            imageView.SetImageResource(_images[position]);

            imageView.Click += (o, e) =>
                {
                //your code here
            };

            //Bitmap image = null;
            //Task.Run(() =>
            //{
            //    URL url = new URL(_imageUrls[position]);
            //    image = BitmapFactory.DecodeStream(url.OpenConnection().InputStream);
            //}).ContinueWith(t =>
            //{
            //    (_context as MainView).RunOnUiThread(() =>
            //    {
            //        imageView.SetImageBitmap(image);
            //    });
            //});


            container.AddView(itemView);
            return itemView;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
        }
    }
}