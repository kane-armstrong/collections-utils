using FluentAssertions;
using System.Linq;
using Xunit;

namespace Armsoft.Collections.Tests.QueryableSortingExtensions.ApplySortSettingsSpec
{
    public class An_unordered_set : TestBase
    {
        [Fact]
        public void sorts_correctly_given_a_single_ascending_sort()
        {
            var settings = new SortSettings
            {
                PropertyName = nameof(MySortableType.Name),
                SortAscending = true
            };
            var set = GenerateTestSet().ApplySortSettings(settings);
            var sut = set.ToArray();
            sut[0].Id.Should().Be(3);
            sut[1].Id.Should().Be(4);
            sut[2].Id.Should().Be(2);
            sut[3].Id.Should().Be(1);
        }

        [Fact]
        public void sorts_correctly_given_a_single_descending_sort()
        {
            var settings = new SortSettings
            {
                PropertyName = nameof(MySortableType.Name),
                SortAscending = false
            };
            var set = GenerateTestSet().ApplySortSettings(settings);
            var sut = set.ToArray();
            sut[0].Id.Should().Be(1);
            sut[1].Id.Should().Be(2);
            sut[2].Id.Should().Be(4);
            sut[3].Id.Should().Be(3);
        }

        [Fact]
        public void sorts_correctly_given_an_ascending_sort_then_ascending_sort()
        {
            var settings = new SortSettings
            {
                PropertyName = nameof(MySortableType.DateOfBirth),
                SortAscending = true,
                ThenBy = new SortSettings
                {
                    PropertyName = nameof(MySortableType.Height),
                    SortAscending = true
                }
            };
            var set = GenerateTestSet().ApplySortSettings(settings);
            var sut = set.ToArray();
            sut[0].Id.Should().Be(1);
            sut[1].Id.Should().Be(2);
            sut[2].Id.Should().Be(4);
            sut[3].Id.Should().Be(3);
        }

        [Fact]
        public void sorts_correctly_given_an_ascending_sort_then_descending_sort()
        {
            var settings = new SortSettings
            {
                PropertyName = nameof(MySortableType.DateOfBirth),
                SortAscending = true,
                ThenBy = new SortSettings
                {
                    PropertyName = nameof(MySortableType.Height),
                    SortAscending = false
                }
            };
            var set = GenerateTestSet().ApplySortSettings(settings);
            var sut = set.ToArray();
            sut[0].Id.Should().Be(1);
            sut[1].Id.Should().Be(2);
            sut[2].Id.Should().Be(3);
            sut[3].Id.Should().Be(4);
        }
    }
}