namespace Microsoft.Toolkit.Uwp.PlatformSpecific
{
    public struct NewMember
    {

        public string Name;

        public int? ParameterCount;

        public NewMember(string s)
        {
            if (s.Length > 2 && s[s.Length - 2] == '#')
            {
                Name = s.Substring(0, s.Length - 2);
                ParameterCount = int.Parse(s.Substring(s.Length - 1));
            }
            else
            {
                Name = s;
                ParameterCount = null;
            }
        }
    }
}
