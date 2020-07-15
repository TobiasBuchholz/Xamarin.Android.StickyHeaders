namespace Xamarin.Android.StickyHeaders
{
    public sealed class SectionItem : ISection
    {
        public SectionItem(int sectionPosition)
        {
            SectionPosition = sectionPosition;
        }

        public SectionType Type => SectionType.Item;
        public int SectionPosition { get; }
    }

}