namespace Xamarin.Android.StickyHeaders
{
    public interface ISection
    {
        SectionType Type { get; }
        int SectionPosition { get; }
    }
}