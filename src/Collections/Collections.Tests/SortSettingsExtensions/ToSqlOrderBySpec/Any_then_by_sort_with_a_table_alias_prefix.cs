using FluentAssertions;
using System;
using Xunit;

namespace Armsoft.Collections.Tests.SortSettingsExtensions.ToSqlOrderBySpec
{
    public class Any_then_by_sort_with_a_table_alias_prefix
    {
        [Fact]
        public void generates_correct_sql_when_both_sorts_have_alias()
        {
            var setting = new SortSettings
            {
                SortAscending = false,
                PropertyName = nameof(SortSettings),
                PropertyTableAlias = "o",
                ThenBy = new SortSettings
                {
                    SortAscending = true,
                    PropertyName = nameof(SortSettings.SortAscending),
                    PropertyTableAlias = "p"
                }
            };
            var sut = setting.ToSqlOrderBy();
            sut.Equals(
                $"order by [o].[{nameof(SortSettings)}] desc, [p].[{nameof(SortSettings.SortAscending)}] asc",
                StringComparison.InvariantCultureIgnoreCase
            ).Should().BeTrue();
        }

        [Fact]
        public void generates_correct_sql_when_initial_sort_has_alias()
        {
            var setting = new SortSettings
            {
                SortAscending = false,
                PropertyName = nameof(SortSettings),
                PropertyTableAlias = "o",
                ThenBy = new SortSettings
                {
                    SortAscending = true,
                    PropertyName = nameof(SortSettings.SortAscending)
                }
            };
            var sut = setting.ToSqlOrderBy();
            sut.Equals(
                $"order by [o].[{nameof(SortSettings)}] desc, [{nameof(SortSettings.SortAscending)}] asc",
                StringComparison.InvariantCultureIgnoreCase
            ).Should().BeTrue();
        }

        [Fact]
        public void generates_correct_sql_when_then_by_sort_has_alias()
        {
            var setting = new SortSettings
            {
                SortAscending = false,
                PropertyName = nameof(SortSettings),
                ThenBy = new SortSettings
                {
                    SortAscending = true,
                    PropertyName = nameof(SortSettings.SortAscending),
                    PropertyTableAlias = "p"
                }
            };
            var sut = setting.ToSqlOrderBy();
            sut.Equals(
                $"order by [{nameof(SortSettings)}] desc, [p].[{nameof(SortSettings.SortAscending)}] asc",
                StringComparison.InvariantCultureIgnoreCase
            ).Should().BeTrue();
        }
    }
}
