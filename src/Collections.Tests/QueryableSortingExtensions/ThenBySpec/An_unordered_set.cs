﻿using FluentAssertions;
using System.Linq;
using Xunit;

namespace Armsoft.Collections.Tests.QueryableSortingExtensions.ThenBySpec;

public class An_unordered_set : TestBase
{
    [Fact]
    public void sorts_correctly()
    {
        var set = GenerateTestSet()
            .OrderBy(nameof(MySortableType.DateOfBirth))
            .ThenBy(nameof(MySortableType.Height));
        var sut = set.ToArray();
        sut[0].Id.Should().Be(1);
        sut[1].Id.Should().Be(2);
        sut[2].Id.Should().Be(4);
        sut[3].Id.Should().Be(3);
    }
}
