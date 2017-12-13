using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
using static Ruling.Rule;

namespace Ruling.Tests.Rules
{
    public class Length_Tests
    {
        [Fact]
        public void StringLength_Should_ThrowArgumentException_When_NoneIsProvided()
        {
            Assert.Throws<ArgumentException>(() => Validate(new Fixture(), Length<Fixture>(f => f.StringValue)));
        }

        [Fact]
        public void ListLength_Should_ThrowArgumentException_When_NoneIsProvided()
        {
            Assert.Throws<ArgumentException>(() => Validate(new Fixture(), Length<Fixture>(f => f.ListValue)));
        }

        [Theory]
        [InlineData("foo", 4, true)]
        [InlineData("foo", 3, true)]
        [InlineData("foo", 2, false)]
        public void StringMaxLength_Should_BeValid_When_ValidationIsOK(string value, int length, bool expected)
        {
            var result = Validate(new Fixture { StringValue = value }, Length<Fixture>(f => f.StringValue, max: length));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void StringMaxLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, max: 3));
            Assert.Equal(string.Format(MaxLengthMessageString, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void StringMaxLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, max: 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void StringMaxLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, max: 3));
            Assert.Equal(nameof(Fixture.StringValue), result.Errors.Single().Key);
        }

        [Fact]
        public void StringMaxLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, max: 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]
        [InlineData("foo", 2, true)]
        [InlineData("foo", 3, true)]
        [InlineData("foo", 4, false)]
        public void StringMinLength_Should_BeValid_When_ValidationIsOK(string value, int length, bool expected)
        {
            var result = Validate(new Fixture { StringValue = value }, Length<Fixture>(f => f.StringValue, min: length));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void StringMinLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, min: 100));
            Assert.Equal(string.Format(MinLengthMessageString, 100), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void StringMinLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, min: 100, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void StringMinLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, min: 100));
            Assert.Equal(nameof(Fixture.StringValue), result.Errors.Single().Key);
        }

        [Fact]
        public void StringMinLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, min: 100, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]
        [InlineData("foo", 3, true)]
        [InlineData("foo", 2, false)]
        [InlineData("foo", 4, false)]
        public void StringExactLength_Should_BeValid_When_ValidationIsOK(string value, int length, bool expected)
        {
            var result = Validate(new Fixture { StringValue = value }, Length<Fixture>(f => f.StringValue, exact: length));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void StringExactLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, exact: 3));
            Assert.Equal(string.Format(ExactLengthMessageString, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void StringExactLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, exact: 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void StringExactLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, exact: 3));
            Assert.Equal(nameof(Fixture.StringValue), result.Errors.Single().Key);
        }

        [Fact]
        public void StringExactLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.StringValue, exact: 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 5, true)]
        [InlineData(new int[] { 1, 2, 3 }, 3, true)]
        [InlineData(new int[] { 1, 2, 3 }, 1, false)]
        public void ListMaxLength_Should_BeValid_When_ValidationIsOK(int[] value, int length, bool expected)
        {
            var result = Validate(new Fixture { ListValue = value }, Length<Fixture>(f => f.ListValue, max: length));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void ListMaxLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, max: 3));
            Assert.Equal(string.Format(MaxLengthMessageList, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void ListMaxLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, max: 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void ListMaxLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, max: 3));
            Assert.Equal(nameof(Fixture.ListValue), result.Errors.Single().Key);
        }

        [Fact]
        public void ListMaxLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, max: 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]

        [InlineData(new int[] { 1, 2, 3 }, 1, true)]
        [InlineData(new int[] { 1, 2, 3 }, 3, true)]
        [InlineData(new int[] { 1, 2, 3 }, 5, false)]
        public void ListMinLength_Should_BeValid_When_ValidationIsOK(int[] value, int length, bool expected)
        {
            var result = Validate(new Fixture { ListValue = value }, Length<Fixture>(f => f.ListValue, min: length));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void ListMinLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, min: 100));
            Assert.Equal(string.Format(MinLengthMessageList, 100), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void ListMinLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, min: 100, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void ListMinLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, min: 100));
            Assert.Equal(nameof(Fixture.ListValue), result.Errors.Single().Key);
        }

        [Fact]
        public void ListMinLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, min: 100, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 3, true)]
        [InlineData(new int[] { 1, 2, 3 }, 5, false)]
        public void ListExactLength_Should_BeValid_When_ValidationIsOK(int[] value, int length, bool expected)
        {
            var result = Validate(new Fixture { ListValue = value }, Length<Fixture>(f => f.ListValue, exact: length));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void ListExactLength_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, exact: 3));
            Assert.Equal(string.Format(ExactLengthMessageList, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void ListExactLength_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, exact: 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void ListExactLength_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, exact: 3));
            Assert.Equal(nameof(Fixture.ListValue), result.Errors.Single().Key);
        }

        [Fact]
        public void ListExactLength_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Length<Fixture>(f => f.ListValue, exact: 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        class Fixture
        {
            public string StringValue { get; set; } = "FooBar";
            public int[] ListValue { get; set; } = new[] { 1, 2, 3, 4, 5 };
        }
    }
}
