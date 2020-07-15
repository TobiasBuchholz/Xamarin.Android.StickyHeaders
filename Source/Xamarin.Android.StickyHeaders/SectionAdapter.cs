using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public class SectionAdapter : RecyclerView.Adapter, IStickyHeaderAdapter
    {
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            if(viewType == (int) SectionType.Header) {
                return new HeaderViewHolder(inflater.Inflate(Resource.Layout.recycler_view_header_item, parent, false));
            } 
            return new ItemViewHolder(inflater.Inflate(Resource.Layout.recycler_view_item, parent, false));
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var type = Items[position].Type;
            var section = Items[position].SectionPosition;

            if(type == SectionType.Header) {
                ((HeaderViewHolder) holder).TextView.Text = $"Header {section}";
            } else {
                ((ItemViewHolder) holder).TextView.Text = $"Item {section}";
            }
        }
        
        public int GetHeaderPositionForItem(int itemPosition)
        {
            return Items[itemPosition].SectionPosition;
        }

        public override int GetItemViewType(int position)
        {
            return (int) Items[position].Type;
        }

        public void OnBindHeaderViewHolder(RecyclerView.ViewHolder viewHolder, int headerPosition)
        {
            ((HeaderViewHolder) viewHolder).TextView.Text = $"Header {headerPosition}";
        }

        public RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent)
        {
            return CreateViewHolder(parent, (int) SectionType.Header) as RecyclerView.ViewHolder;
        }

        public IList<ISection> Items { private get; set; } = new List<ISection>();
        public override int ItemCount => Items.Count;

        private class HeaderViewHolder : RecyclerView.ViewHolder
        {
            public HeaderViewHolder(View itemView) 
                : base(itemView)
            {
                TextView = itemView.FindViewById<TextView>(Resource.Id.text_view);
            }

            public TextView TextView { get; }
        }
        
        private class ItemViewHolder : RecyclerView.ViewHolder
        {
            public ItemViewHolder(View itemView) 
                : base(itemView)
            {
                TextView = itemView.FindViewById<TextView>(Resource.Id.text_view);
            }

            public TextView TextView { get; }
        }
    }
}