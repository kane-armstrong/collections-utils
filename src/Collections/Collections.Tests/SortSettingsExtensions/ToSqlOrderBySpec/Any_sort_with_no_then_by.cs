using FluentAssertions;
using System;
using Xunit;

namespace Armsoft.Collections.Tests.SortSettingsExtensions.ToSqlOrderBySpec
{
    public class Any_sort_with_no_then_by
    {
        [Fact]
        public void should_generate_correct_ascending_string_representation()
        {
            var setting = new SortSettings
            {
                SortAscending = true,
                PropertyName = nameof(SortSettings),
                ThenBy = null
            };
            var sut = setting.ToSqlOrderBy();
            sut.Equals(
                $"order by [{nameof(SortSettings)}] asc",
                StringComparison.InvariantCultureIgnoreCase
            ).Should().BeTrue();
        }

        [Fact]
        public void should_generate_correct_descending_string_representation()
        {
            var setting = new SortSettings
            {
                SortAscending = false,
                PropertyName = nameof(SortSettings),
                ThenBy = null
            };
            var sut = setting.ToSqlOrderBy();
            sut.Equals(
                $"order by [{nameof(SortSettings)}] desc",
                StringComparison.InvariantCultureIgnoreCase
            ).Should().BeTrue();
        }
    }
}