namespace Dapper.FluentMap.Tests
{
    public class TestEntity
    {
        public int Id { get; set; }
    }

    public class DerivedTestEntity : TestEntity
    {
        public string Name { get; set; }
    }
}
