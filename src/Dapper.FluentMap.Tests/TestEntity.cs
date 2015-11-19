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

    public class TestEntityWithEqualPropertyNameAsColumn
    {
        public int Id { get; set; }

        public string Position { get; private set; }

        private ulong positionAsInt64;
        public ulong PositionAsInt64
        {
            get { return positionAsInt64; }
            set
            {
                positionAsInt64 = value;
                Position = string.Format("POS: " + positionAsInt64);
            }
        }
    }
}