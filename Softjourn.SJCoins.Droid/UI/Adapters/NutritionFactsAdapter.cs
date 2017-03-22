
using System.Collections.Generic;
using System.Linq;

using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using Thread = System.Threading.Thread;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    class NutritionFactsAdapter : RecyclerView.Adapter
    {
        private Dictionary<string,string> _nutritionFacts = new Dictionary<string, string>();

        public void SetData(Dictionary<string,string> nutritionFacts)
        {
            _nutritionFacts = nutritionFacts;
            NotifyDataSetChanged();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as NutritionViewHolder;

            if (holder == null) return;
            holder._name.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase( _nutritionFacts.Keys.ElementAt(position));
            holder._value.Text = _nutritionFacts.Values.ElementAt(position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.recycler_nutrition_fact, parent, false);
            return new NutritionViewHolder(v);
        }

        public override int ItemCount => _nutritionFacts?.Count ?? 0;
    }

    public class NutritionViewHolder : RecyclerView.ViewHolder
    {
        public TextView _name;
        public TextView _value;

        public NutritionViewHolder(View v) : base(v)
        {
            _name = v.FindViewById<TextView>(Resource.Id.details_info_fact_name);
            _value = v.FindViewById<TextView>(Resource.Id.details_info_fact_value);
        }
    }
}