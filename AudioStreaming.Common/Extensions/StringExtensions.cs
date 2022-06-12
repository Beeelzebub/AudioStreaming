namespace AudioStreaming.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GetTrackPathInStorage(this string source)
        {
            var splitedPath = source.Split('/');

            return splitedPath.Length > 2
                ? '/' + splitedPath[splitedPath.Length - 2] + '/' + splitedPath.Last()
                : string.Empty;
        }

    }
}
