using FluentAssertions;
using System;
using Xunit;

namespace Armsoft.Collections.Tests.SortSettingsExtensions.ToSqlOrderBySpec
{
    public class Any_sort_with_a_table_alias_prefix
    {
        [Fact]
        public void should_generate_correct_table_aliased_string_representation()
        {
            const string alias = "o";
            var setting = new SortSettings
            {
                SortAscending = true,
                PropertyName = nameof(SortSettings),
                ThenBy = null,
                PropertyTableAlias = alias
            };
            var sut = setting.ToSqlOrderBy();
            sut.Equals(
                $"order by [{alias}].[{nameof(SortSettings)}] asc",
                StringComparison.InvariantCultureIgnoreCase
            ).Should().BeTrue();
        }
    }
}
