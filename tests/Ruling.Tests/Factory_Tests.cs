using System;
using System.Linq;
using Xunit;
using static Ruling.Factory;

namespace Ruling.Tests
{
    public class Factory_Tests
    {
        [Fact]
        public void Factory_Should_ThrowArgumentNullException_When_InputIsNull()
        {
            var factory = CreateRuling<Fixture>(
                f => new Result()
            );

            Assert.Throws<ArgumentNullException>(() => factory(null));
        }

        [Fact]
        public void Factory_Should_PassInputToAllValidators()
        {
            var fixture = new Fixture();
            var validatorsCount = 0;
            CreateRuling<Fixture>(
                f =>
                {
                    validatorsCount++;
                    Assert.Equal(fixture, fixture);
                    return new Result();
                },
                f =>
                {
                    validatorsCount++;
                    Assert.Equal(fixture, fixture);
                    return new Result();
                }
            )(fixture);

            Assert.Equal(2, validatorsCount);
        }

        [Fact]
        public void Factory_Should_CombineResults()
        {
            var factory = CreateRuling<Fixture>(
                f => new Result(new[] { (true, "key1", "message1") }),
                f => new Result(new[] { (false, "key2", "message2") })
            );

            var result = factory(new Fixture());
            Assert.False(result.Valid);
            Assert.Single(result.Errors);
            Assert.Equal(2, result.Messages.Count);
        }

        class Fixture
        {
            public int Id { get; set; } = 1;
        }
    }
}
