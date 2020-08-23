using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.FluentMap.Dommel.Tests
{
    public class TestEntity
    {
        public int Id { get; set; }

        public int? OtherId { get; set; }
    }

    public class CustomIdEntity
    {
        public int CustomId { get; set; }
        public string Name { get; set; }
    }

    public class DoubleIdEntity
    {
        public int Id { get; set; }
        public int CustomId { get; set; }
        public string Name { get; set; }
    }

    public class Other
    {
        public int Id { get; set; }
    }
}
