using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public interface IStickyHeaderAdapter 
    {
        int GetHeaderPositionForItem(int itemPosition);
        void OnBindHeaderViewHolder(RecyclerView.ViewHolder viewHolder, int headerPosition);
        RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent);
    }
}