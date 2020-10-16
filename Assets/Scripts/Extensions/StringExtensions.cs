namespace Assets.Scripts.Extensions
{
    public static class StringExtensions
    {
        public static string FormatCharacterName(this string objectName)
        {
            return objectName.Substring(0, objectName.IndexOf("(")).Trim();
        }
    }
}
