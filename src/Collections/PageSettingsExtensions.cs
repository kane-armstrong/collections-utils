namespace Armsoft.Collections
{
    public static class PageSettingsExtensions
    {
        public static string ToSqlPaginate(this PageSettings page)
        {
            var offset = (page.Number - 1) * page.Size;
            return $"offset {offset} rows fetch next {page.Size} rows only";
        }
    }
}