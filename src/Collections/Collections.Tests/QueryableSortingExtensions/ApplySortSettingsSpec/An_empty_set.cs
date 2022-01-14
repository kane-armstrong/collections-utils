using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Armsoft.Collections.Tests.QueryableSortingExtensions.ApplySortSettingsSpec;

public class An_empty_set : TestBase
{
    [Fact]
    public void allows_any_sorting_attempt()
    {
        var settings = new SortSettings
        {
            PropertyName = nameof(MySortableType.Name),
            SortAscending = true
        };
        var set = new List<MySortableType>().AsQueryable().ApplySortSettings(settings);
        var sut = set.ToList();
        sut.Should().BeEmpty();
    }
}
