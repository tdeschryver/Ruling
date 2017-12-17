using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
using static Ruling.Rule;
using System.Text.RegularExpressions;

namespace Ruling.Tests.Rules
{
    public class Format_Tests
    {
        [Theory]
        [InlineData("123-555-0190", "^\\d{3}-\\d{3}-\\d{4}$", true)]
        [InlineData("444-234-22450", "^\\d{3}-\\d{3}-\\d{4}$", false)]
        [InlineData(null, "foo", false)]
        [InlineData("foo", null, false)]
        public void Format_Should_BeValid_When_ValidationIsOK(string value, string regex, bool expected)
        {
            var result = Validate(new Fixture { Value = value }, Format<Fixture>(f => f.Value, regex));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void Format_Should_UseRegexOptions_When_Provided()
        {
            var result = Validate(new Fixture { Value = "FOO" }, Format<Fixture>(f => f.Value, "foo", RegexOptions.IgnoreCase));
            Assert.True(result.Valid);
        }

        [Fact]
        public void Format_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Format<Fixture>(f => f.Value, "\\S"));
            Assert.Equal(string.Format(FormatMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Format_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Format<Fixture>(f => f.Value, "\\S", message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Format_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Format<Fixture>(f => f.Value, "\\S"));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void Format_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Format<Fixture>(f => f.Value, "\\S", key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        class Fixture
        {
            public string Value { get; set; }
        }
    }
}
