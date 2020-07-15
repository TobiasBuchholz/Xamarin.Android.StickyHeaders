using System;
using Android.Graphics;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    public sealed class StickyHeaderItemDecoration : RecyclerView.ItemDecoration
    {
        private readonly StickyHeaderAdapter _adapter;
        private int _currentStickyPosition = RecyclerView.NoPosition;
        private RecyclerView _recyclerView;
        private RecyclerView.ViewHolder _currentStickyHolder;
        private View _lastViewOverlappedByHeader;

        public StickyHeaderItemDecoration(StickyHeaderAdapter adapter)
        {
            _adapter = adapter;
        }

        public void AttachToRecyclerView(RecyclerView recyclerView)
        {
            if(_recyclerView == recyclerView) {
                return; 
            }
            
            if(_recyclerView != null) {
                DestroyCallbacks(_recyclerView);
            }

            _recyclerView = recyclerView;
            if(recyclerView != null) {
                _currentStickyHolder = _adapter.OnCreateHeaderViewHolder(recyclerView);
                FixLayoutSize();
                SetupCallbacks();
            }

        }

        private void DestroyCallbacks(RecyclerView recyclerView)
        {
            recyclerView.RemoveItemDecoration(this);
        }

        private void FixLayoutSize()
        {
            _recyclerView.ViewTreeObserver.AddOnGlobalLayoutListener(new OnGlobalLayoutListener(x => {
                _recyclerView.ViewTreeObserver.RemoveOnGlobalLayoutListener(x);
                
                // specs for parent (RecyclerView)
                var widthSpec = View.MeasureSpec.MakeMeasureSpec(_recyclerView.Width, MeasureSpecMode.Exactly);
                var heightSpec = View.MeasureSpec.MakeMeasureSpec(_recyclerView.Height, MeasureSpecMode.Unspecified);
                
                // specs for children (headers)
                var childWidthSpec = ViewGroup.GetChildMeasureSpec(
                    widthSpec,
                    _recyclerView.PaddingLeft + _recyclerView.PaddingRight,
                    _currentStickyHolder.ItemView.LayoutParameters.Width);

                var childHeightSpec = ViewGroup.GetChildMeasureSpec(
                    heightSpec,
                    _recyclerView.PaddingTop + _recyclerView.PaddingBottom,
                    _currentStickyHolder.ItemView.LayoutParameters.Height);
                
                _currentStickyHolder.ItemView.Measure(childWidthSpec, childHeightSpec);
                
                _currentStickyHolder.ItemView.Layout(0, 0, _currentStickyHolder.ItemView.MeasuredWidth, _currentStickyHolder.ItemView.MeasuredHeight);
            }));
        }

        private void SetupCallbacks()
        {
            _recyclerView.AddItemDecoration(this);
        }

        public override void OnDrawOver(Canvas canvas, RecyclerView parent, RecyclerView.State state)
        {
            base.OnDrawOver(canvas, parent, state);

            var layoutManager = parent.GetLayoutManager();
            if(layoutManager == null) {
                return;
            }

            var topChildPosition = RecyclerView.NoPosition;
            if(layoutManager is LinearLayoutManager manager) {
                topChildPosition = manager.FindFirstVisibleItemPosition();
            } else {
                var topChild = parent.GetChildAt(0);
                if(topChild != null) {
                    topChildPosition = parent.GetChildAdapterPosition(topChild);
                }
            }

            if(topChildPosition == RecyclerView.NoPosition) {
                return;
            }

            var viewOverlappedByHeader = GetChildInContact(parent, _currentStickyHolder.ItemView.Bottom) ?? (_lastViewOverlappedByHeader ?? parent.GetChildAt(topChildPosition));
            _lastViewOverlappedByHeader = viewOverlappedByHeader;

            var overlappedByHeaderPosition = parent.GetChildAdapterPosition(viewOverlappedByHeader);
            var overlappedHeaderPosition = 0;
            var preOverlappedPosition = 0;
            if(overlappedByHeaderPosition > 0) {
                preOverlappedPosition = _adapter.GetHeaderPositionForItem(overlappedByHeaderPosition - 1);
                overlappedHeaderPosition = _adapter.GetHeaderPositionForItem(overlappedByHeaderPosition);
            } else {
                preOverlappedPosition = _adapter.GetHeaderPositionForItem(topChildPosition);
                overlappedHeaderPosition = preOverlappedPosition;
            }

            if(preOverlappedPosition == RecyclerView.NoPosition) {
                return;
            }

            if(preOverlappedPosition != overlappedHeaderPosition && ShouldMoveHeader(viewOverlappedByHeader)) {
                UpdateStickyHeader(topChildPosition, overlappedByHeaderPosition);
                MoveHeader(canvas, viewOverlappedByHeader);
            } else {
                UpdateStickyHeader(topChildPosition, RecyclerView.NoPosition);
                DrawHeader(canvas);
            }
        }

        private static bool ShouldMoveHeader(View viewOverlappedByHeader)
        {
            var dy = viewOverlappedByHeader.Top - viewOverlappedByHeader.Height;
            return viewOverlappedByHeader.Top >= 0 && dy <= 0;
        }

        private void UpdateStickyHeader(int topChildPosition, int contactChildPosition)
        {
            var headerPositionForItem = _adapter.GetHeaderPositionForItem(topChildPosition);
            if(headerPositionForItem != _currentStickyPosition && headerPositionForItem != RecyclerView.NoPosition) {
                _adapter.OnBindHeaderViewHolder(_currentStickyHolder, headerPositionForItem);
                _currentStickyPosition = headerPositionForItem;
            } else if(headerPositionForItem != RecyclerView.NoPosition) {
                _adapter.OnBindHeaderViewHolder(_currentStickyHolder, headerPositionForItem);
            }
        }

        private void MoveHeader(Canvas canvas, View nextHeader)
        {
            canvas.Save();
            canvas.Translate(0, nextHeader.Top - nextHeader.Height);
            _currentStickyHolder.ItemView.Draw(canvas);
            canvas.Restore();
        }

        private void DrawHeader(Canvas canvas)
        {
            canvas.Save();
            canvas.Translate(0, 0);
            _currentStickyHolder.ItemView.Draw(canvas);
            canvas.Restore();
        }

        private static View GetChildInContact(ViewGroup parent, int contactPoint)
        {
            for(var i = 0; i < parent.ChildCount; i++) {
                var child = parent.GetChildAt(i);
                if(child.Bottom > contactPoint) {
                    if(child.Top <= contactPoint) {
                        return child;
                    }
                }
            }
            return null;
        }
    }
}