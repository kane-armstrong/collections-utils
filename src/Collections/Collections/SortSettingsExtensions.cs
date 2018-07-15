using System.Text;

namespace Armsoft.Collections
{
    public static class SortSettingsExtensions
    {
        public static string ToSqlOrderBy(this SortSettings sort)
        {
            var builder = new StringBuilder();
            builder.Append($"order by {sort.PropertyName} {SortDirectionAsSqlSort(sort.SortAscending)}");

            var then = sort.ThenBy;
            while (then != null)
            {
                builder.Append($", {then.PropertyName} {SortDirectionAsSqlSort(then.SortAscending)}");
                then = then.ThenBy;
            }

            return builder.ToString();
        }

        private static string SortDirectionAsSqlSort(bool flag)
        {
            return flag ? "asc" : "desc";
        }
    }
}