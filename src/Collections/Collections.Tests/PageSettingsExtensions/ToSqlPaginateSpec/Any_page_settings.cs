using FluentAssertions;
using System;
using Xunit;

namespace Armsoft.Collections.Tests.PageSettingsExtensions.ToSqlPaginateSpec;

public class Any_page_settings
{
    [Fact]
    public void should_produce_correct_paging_clause()
    {
        var page = new PageSettings { Number = 10, Size = 5 };
        var sut = page.ToSqlPaginate();
        sut.Equals(
            "offset 45 rows fetch next 5 rows only",
            StringComparison.InvariantCultureIgnoreCase
        ).Should().BeTrue();
    }
}
