using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public sealed class SimpleItemAdapter : RecyclerView.Adapter, IStickyHeaderAdapter
    {
        private readonly SectionIndexAdapterDelegate<SimpleItem> _adapterDelegate;

        public SimpleItemAdapter()
        {
            _adapterDelegate = new SectionIndexAdapterDelegate<SimpleItem>(
                CreateHeaderViewHolder,
                CreateItemViewHolder,
                BindHeaderViewHolder,
                BindItemViewHolder);
        }

        private static RecyclerView.ViewHolder CreateHeaderViewHolder(ViewGroup parent)
        {
            return new HeaderViewHolder(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.recycler_view_header_item, parent, false));
        }
        
        private static RecyclerView.ViewHolder CreateItemViewHolder(ViewGroup parent)
        {
            return new ItemViewHolder(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.recycler_view_item, parent, false));
        }

        private static void BindHeaderViewHolder(RecyclerView.ViewHolder viewHolder, SimpleItem item)
        {
            ((HeaderViewHolder) viewHolder).TextView.Text = $"Header for: {item.Title}";
        }

        private static void BindItemViewHolder(RecyclerView.ViewHolder viewHolder, SimpleItem item)
        {
            ((ItemViewHolder) viewHolder).TextView.Text = item.Title;
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _adapterDelegate.OnCreateViewHolder(parent, viewType);
        }

        public RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent)
        {
            return CreateViewHolder(parent, SectionIndexAdapterDelegate.ViewTypeHeader) as RecyclerView.ViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _adapterDelegate.OnBindViewHolder(holder, position);
        }

        public void OnBindHeaderViewHolder(RecyclerView.ViewHolder viewHolder, int headerPosition)
        {
            BindHeaderViewHolder(viewHolder, _adapterDelegate.GetItem(headerPosition));
        }

        public override int GetItemViewType(int position)
        {
            return _adapterDelegate.GetItemViewType(position);
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