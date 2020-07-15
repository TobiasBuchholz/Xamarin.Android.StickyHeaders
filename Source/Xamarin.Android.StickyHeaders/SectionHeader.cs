namespace Xamarin.Android.StickyHeaders
{
    public sealed class SectionHeader : ISection
    {
        public SectionHeader(int sectionPosition)
        {
            SectionPosition = sectionPosition;
        }

        public SectionType Type => SectionType.Header;
        public int SectionPosition { get; }
    }
}