using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Android.StickyHeaders
{
    public sealed class SectionIndexAdapterDelegate<T>
    {
        public T GetItem(int position)
        {
            var previousHeaderCount = SectionIndexes.Count(x => x < position);
            return Items[position - previousHeaderCount];
        }

        public bool IsHeader(int position)
        {
            return SectionIndexes.Contains(position);
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
}