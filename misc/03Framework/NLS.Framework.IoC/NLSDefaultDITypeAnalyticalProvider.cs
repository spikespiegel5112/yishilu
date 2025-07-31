namespace NLS.Framework.IoC
{
    public sealed class NLSDefaultDITypeAnalyticalProvider : INLSDITypeAnalyticalProvider
    {
        public INLSIDITypeAnalytical CreateDITypeAnalaytical()
        {
            return new NLSIDITypeAnalytical();
        }
    }
}