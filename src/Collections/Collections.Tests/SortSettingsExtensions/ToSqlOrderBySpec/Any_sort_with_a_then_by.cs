using FluentAssertions;
using System;
using Xunit;

namespace Armsoft.Collections.Tests.SortSettingsExtensions.ToSqlOrderBySpec
{
    public class Any_sort_with_a_then_by
    {
        [Fact]
        public void should_generate_correct_string_representation()
        {
            var setting = new SortSettings
            {
                SortAscending = false,
                PropertyName = nameof(SortSettings),
                ThenBy = new SortSettings
                {
                    SortAscending = true,
                    PropertyName = nameof(SortSettings.SortAscending)
                }
            };
            var sut = setting.ToSqlOrderBy();
            sut.Equals(
                $"order by {nameof(SortSettings)} desc, {nameof(SortSettings.SortAscending)} asc",
                StringComparison.InvariantCultureIgnoreCase
            ).Should().BeTrue();
        }
    }
}