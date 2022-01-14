using System.Text;

namespace Armsoft.Collections;

public static class SortSettingsExtensions
{
    public static string ToSqlOrderBy(this SortSettings sort)
    {
        var builder = new StringBuilder();
        var prefix = string.IsNullOrEmpty(sort.PropertyTableAlias) ? "" : $"[{sort.PropertyTableAlias}].";
        builder.Append($"order by {prefix}[{sort.PropertyName}] {SortDirectionAsSqlSort(sort.SortAscending)}");

        var then = sort.ThenBy;
        while (then != null)
        {
            var thenPrefix = string.IsNullOrEmpty(then.PropertyTableAlias) ? "" : $"[{then.PropertyTableAlias}].";
            builder.Append($", {thenPrefix}[{then.PropertyName}] {SortDirectionAsSqlSort(then.SortAscending)}");
            then = then.ThenBy;
        }

        return builder.ToString();
    }

    private static string SortDirectionAsSqlSort(bool flag)
    {
        return flag ? "asc" : "desc";

    }
}
