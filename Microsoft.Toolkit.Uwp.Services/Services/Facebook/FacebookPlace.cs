namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    public class FacebookPlace
    {
        public static string Fields => "id, name";

        public string Id { get; set; }

        public string Name { get; set; }
    }
}