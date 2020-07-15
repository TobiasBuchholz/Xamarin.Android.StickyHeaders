using System;
using Android.Views;
using Object = Java.Lang.Object;

namespace Xamarin.Android.StickyHeaders
{
    public class OnGlobalLayoutListener : Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly Action<OnGlobalLayoutListener> _action;

        public OnGlobalLayoutListener(Action<OnGlobalLayoutListener> action)
        {
            _action = action;
        }

        public void OnGlobalLayout()
        {
            _action(this);
        }
    }
}