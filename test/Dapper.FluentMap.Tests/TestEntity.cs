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

    public class ValueObjectTestEntity
    {
        public EmailTestValueObject Email { get; set; }
    }

    public class EmailTestValueObject
    {
        public string Address { get; set; }
    }
}
