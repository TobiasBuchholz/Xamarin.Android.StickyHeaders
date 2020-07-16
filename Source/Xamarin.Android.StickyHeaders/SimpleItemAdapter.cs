using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public sealed class SimpleItemAdapter : RecyclerView.Adapter, IStickyHeaderAdapter
    {
        private const int ViewTypeHeader = 0;
        private const int ViewTypeItem = 1;
        
        private readonly SectionIndexAdapterDelegate<SimpleItem> _adapterDelegate;

        public SimpleItemAdapter()
        {
            _adapterDelegate = new SectionIndexAdapterDelegate<SimpleItem>();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            if(viewType == ViewTypeHeader) {
                return new HeaderViewHolder(inflater.Inflate(Resource.Layout.recycler_view_header_item, parent, false));
            } 
            return new ItemViewHolder(inflater.Inflate(Resource.Layout.recycler_view_item, parent, false));
        }

        public RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent)
        {
            return CreateViewHolder(parent, ViewTypeHeader) as RecyclerView.ViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _adapterDelegate.GetItem(position);
            
            if(_adapterDelegate.IsHeader(position)) {
                ((HeaderViewHolder) holder).TextView.Text = $"Header for: {item.Title}";
            } else {
                ((ItemViewHolder) holder).TextView.Text = item.Title;
            }
        }

        public void OnBindHeaderViewHolder(RecyclerView.ViewHolder viewHolder, int headerPosition)
        {
            var item = _adapterDelegate.GetItem(headerPosition);
            ((HeaderViewHolder) viewHolder).TextView.Text = $"Header for: {item.Title}";
        }

        public override int GetItemViewType(int position)
        {
            return _adapterDelegate.IsHeader(position) ? ViewTypeHeader : ViewTypeItem;
        }

        public override int ItemCount => _adapterDelegate.ItemCount;

        public IList<SimpleItem> Items {
            set => _adapterDelegate.Items = value;
        }

        public IList<int> SectionIndexes {
            set => _adapterDelegate.SectionIndexes = value;
        }

        public int GetHeaderPositionForItem(int itemPosition)
        {
            return _adapterDelegate.GetHeaderPositionForItem(itemPosition);
        }
        
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