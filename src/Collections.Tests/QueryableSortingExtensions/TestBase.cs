using System.Collections.Generic;
using System.Linq;

namespace Armsoft.Collections.Tests.QueryableSortingExtensions
{
    public class TestBase
    {
        protected class MySortableType
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Height { get; set; }
            public int DateOfBirth { get; set; }
        }

        protected IQueryable<MySortableType> GenerateTestSet()
        {
            return new List<MySortableType>
            {
                new MySortableType { Id = 1, Name = "John Doe", DateOfBirth = 1950, Height = 6.4},
                new MySortableType { Id = 2, Name = "Jane Doe", DateOfBirth = 1965, Height = 4.9},
                new MySortableType { Id = 3, Name = "Albert Doe", DateOfBirth = 1985, Height = 5.8},
                new MySortableType { Id = 4, Name = "Alexa Doe", DateOfBirth = 1985, Height = 5.7}
            }.AsQueryable();
        }
    }
}