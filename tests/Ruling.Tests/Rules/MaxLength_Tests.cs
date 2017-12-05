using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Factory;
using static Ruling.Rule;

namespace Ruling.Tests.Rules
{
    public class MaxLength_Tests
    {
        [Theory]
        [InlineData("foo", 4, true)]
        [InlineData("foo", 3, true)]
        [InlineData("foo", 2, false)]
        public void MaxLength_Should_BeValid_When_ValidationIsOK(string value, int maxLength, bool expected)
        {
            var ruling = CreateRuling(MaxLength<Fixture>(f => f.Value, maxLength));
            var result = ruling(new Fixture { Value = value });

            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void MaxLength_Should_BeInvalid_When_InputIsNull()
        {
            var ruling = CreateRuling(MaxLength<Fixture>(f => f.Value, 3));
            var result = ruling(null);

            Assert.False(result.Valid);
        }

        [Fact]
        public void MaxLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var ruling = CreateRuling(MaxLength<Fixture>(f => f.Value, 3));
            var result = ruling(new Fixture());

            Assert.Equal(string.Format(MaxLengthMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void MaxLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var ruling = CreateRuling(MaxLength<Fixture>(f => f.Value, 3, message: "Custom message"));
            var result = ruling(new Fixture());

            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void MaxLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var ruling = CreateRuling(MaxLength<Fixture>(f => f.Value, 3));
            var result = ruling(new Fixture());

            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void MaxLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var ruling = CreateRuling(MaxLength<Fixture>(f => f.Value, 3, key: "Foooo"));
            var result = ruling(new Fixture());

            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        class Fixture
        {
            public string Value { get; set; } = "FooBar";
        }
    }
}
