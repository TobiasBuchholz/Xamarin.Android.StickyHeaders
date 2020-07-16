using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public sealed class SectionIndexAdapterDelegate<T> : SectionIndexAdapterDelegate
    {
        private readonly Func<ViewGroup, RecyclerView.ViewHolder> _createHeaderViewHolderFunc;
        private readonly Func<ViewGroup, RecyclerView.ViewHolder> _createItemViewHolderFunc;
        private readonly Action<RecyclerView.ViewHolder, T> _bindHeaderViewHolderFunc;
        private readonly Action<RecyclerView.ViewHolder, T> _bindItemViewHolderFunc;

        public SectionIndexAdapterDelegate(
            Func<ViewGroup, RecyclerView.ViewHolder> createHeaderViewHolderFunc,
            Func<ViewGroup, RecyclerView.ViewHolder> createItemViewHolderFunc,
            Action<RecyclerView.ViewHolder, T> bindHeaderViewHolderFunc,
            Action<RecyclerView.ViewHolder, T> bindItemViewHolderFunc)
        {
            _createHeaderViewHolderFunc = createHeaderViewHolderFunc;
            _createItemViewHolderFunc = createItemViewHolderFunc;
            _bindHeaderViewHolderFunc = bindHeaderViewHolderFunc;
            _bindItemViewHolderFunc = bindItemViewHolderFunc;
        }

        public RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return viewType == ViewTypeHeader
                ? _createHeaderViewHolderFunc(parent)
                : _createItemViewHolderFunc(parent);
        }

        public void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetItem(position);
            
            if(IsHeader(position)) {
                _bindHeaderViewHolderFunc(holder, item);
            } else {
                _bindItemViewHolderFunc(holder, item);
            }
        }
        
        public T GetItem(int position)
        {
            var previousHeaderCount = SectionIndexes.Count(x => x < position);
            return Items[position - previousHeaderCount];
        }
        
        private bool IsHeader(int position)
        {
            return SectionIndexes.Contains(position);
        }

        public int GetItemViewType(int position)
        {
            return IsHeader(position) ? ViewTypeHeader : ViewTypeItem;
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
        
        public int ItemCount => Items.Count + SectionIndexes.Count;
        public IList<T> Items { get; set; } = new List<T>();
        public IList<int> SectionIndexes { get; set; } = new List<int>();
    }

    public abstract class SectionIndexAdapterDelegate
    {
        public const int ViewTypeHeader = 0;
        public const int ViewTypeItem = 1;
    }
}