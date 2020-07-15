using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public class SectionIndexAdapter<T> : RecyclerView.Adapter, IStickyHeaderAdapter
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
            var previousHeaderCount = GetPreviousHeaderCount(position);
            var itemIndex = position - previousHeaderCount;
            
            if(IsHeader(position, previousHeaderCount)) {
                ((HeaderViewHolder) holder).TextView.Text = $"Header {itemIndex}";
            } else {
                ((ItemViewHolder) holder).TextView.Text = $"Item {itemIndex}";
            }
        }

        private int GetPreviousHeaderCount(int position)
        {
            return SectionIndexes.Count(x => x < position);
        }

        private bool IsHeader(int position, int previousHeaderCount)
        {
            return position == (previousHeaderCount >= SectionIndexes.Count ? SectionIndexes.Last() : SectionIndexes[previousHeaderCount]);
        }

        public int GetHeaderPositionForItem(int itemPosition)
        {
            for(var i = 0; i < SectionIndexes.Count; i++) {
                var sectionIndex = SectionIndexes[i];
                if(itemPosition < sectionIndex) {
                    return SectionIndexes[i - 1];
                }
            }
            return SectionIndexes.Last();
        }
        
        public override int GetItemViewType(int position)
        {
            return IsHeader(position) ? (int) SectionType.Header : (int) SectionType.Item;
        }

        private bool IsHeader(int position)
        {
            return IsHeader(position, GetPreviousHeaderCount(position));
        }

        public void OnBindHeaderViewHolder(RecyclerView.ViewHolder viewHolder, int headerPosition)
        {
            var itemIndex = headerPosition - GetPreviousHeaderCount(headerPosition);
            ((HeaderViewHolder) viewHolder).TextView.Text = $"Header {itemIndex}";
        }

        public RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent)
        {
            return CreateViewHolder(parent, (int) SectionType.Header) as RecyclerView.ViewHolder;
        }

        public override int ItemCount => Items.Count + SectionIndexes.Count;
        public IList<T> Items { get; set; } = new List<T>();
        public IList<int> SectionIndexes { get; set; } = new List<int>();
        
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