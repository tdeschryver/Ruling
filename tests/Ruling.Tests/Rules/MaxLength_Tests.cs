using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
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
            var result = Validate(new Fixture { Value = value }, MaxLength<Fixture>(f => f.Value, maxLength));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void MaxLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), MaxLength<Fixture>(f => f.Value, 3));
            Assert.Equal(string.Format(MaxLengthMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void MaxLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), MaxLength<Fixture>(f => f.Value, 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void MaxLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), MaxLength<Fixture>(f => f.Value, 3));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void MaxLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), MaxLength<Fixture>(f => f.Value, 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        class Fixture
        {
            public string Value { get; set; } = "FooBar";
        }
    }
}
