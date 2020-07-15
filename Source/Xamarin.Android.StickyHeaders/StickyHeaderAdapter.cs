using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public abstract class StickyHeaderAdapter : RecyclerView.Adapter 
    {
        public abstract int GetHeaderPositionForItem(int itemPosition);
        public abstract void OnBindHeaderViewHolder(RecyclerView.ViewHolder viewHolder, int headerPosition);
        public abstract RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent);
    }
}