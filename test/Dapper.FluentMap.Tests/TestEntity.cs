namespace Dapper.FluentMap.Tests
{
    public class TestEntity
    {
        public int Id { get; set; }

        public int? OtherId { get; set; }
    }

    public class TestEntityWithNullable
    {
        public int Id { get; set; }

        public decimal? Value { get; set; }
    }

    public class DerivedTestEntity : TestEntity
    {
        public string Name { get; set; }
    }

    public class ValueObjectTestEntity
    {
        public EmailTestValueObject Email { get; set; }

        public SameMemberAsStringObject String { get; set; }
    }

    public class EmailTestValueObject
    {
        public string Address { get; set; }
    }

    public class SameMemberAsStringObject
    {
        public string Length { get; set; }
    }
}
