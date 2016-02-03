using Nancy.Testing;

namespace tests
{
    public static class NancyTestingExtensions
    {
        public static string BodyAsText(this BrowserResponse response)
        {
            using (var contentsStream = new System.IO.MemoryStream())
            {
                response.Context.Response.Contents.Invoke(contentsStream);
                contentsStream.Position = 0;
                return System.Text.Encoding.ASCII.GetString(contentsStream.ToArray());
            }
        }
    }
}
