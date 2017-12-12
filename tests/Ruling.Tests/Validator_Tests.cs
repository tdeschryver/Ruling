using System;
using System.Linq;
using Xunit;
using static Ruling.Validator;
using static Ruling.Rule;

namespace Ruling.Tests
{
    public class Validator_Tests
    {
        [Fact]
        public void Validate_Should_BeValid_When_RulesAreOK()
        {
            var result = Validate(new Fixture(), _fooRequired, _barRequired);
            Assert.True(result.Valid);
        }

        [Fact]
        public void Validate_Should_BeInvalid_When_RulesAreNotOK()
        {
            var result = Validate(new Fixture(false), _fooRequired, _barRequired);
            Assert.False(result.Valid);
        }

        [Fact]
        public void Validate_Should_HaveNoErrors_When_Valid()
        {
            var result = Validate(new Fixture(), _fooRequired, _barRequired);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_Should_HaveErrors_When_Invalid()
        {
            var result = Validate(new Fixture(false), _fooRequired, _barRequired);
            Assert.Equal(2, result.Errors.Count);
        }

        [Fact]
        public void GreaterThan_Should_ThrowArgumentNullException_When_InputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Validate(null, _fooRequired));
        }

        Func<Fixture, (bool valid, string key, string message)> _fooRequired =
            f => (!string.IsNullOrWhiteSpace(f.Foo), nameof(Fixture.Foo), "Foo is required");

        Func<Fixture, (bool valid, string key, string message)> _barRequired =
            f => (!string.IsNullOrWhiteSpace(f.Bar), nameof(Fixture.Bar), "Bar is required");

        class Fixture
        {
            public Fixture(bool valid = true)
            {
                Foo = valid ? Foo : string.Empty;
                Bar = valid ? Bar : string.Empty;
            }

            public string Foo { get; set; } = "foo";
            public string Bar { get; set; } = "bar";
        }
    }
}
