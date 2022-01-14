using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Armsoft.Collections.Tests.PaginatedList;

public class An_empty_list
{
    [Fact]
    public void returns_an_empty_list_when_tolist_invoked()
    {
        var sut = new PaginatedList<string>(new List<string>(), 0, 2, 2);
        var source = sut.ToList();
        source.Should().BeEmpty();
    }
}
