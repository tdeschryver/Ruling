using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
using static Ruling.Rule;

namespace Ruling.Tests.Rules
{
    public class GreaterThan_Tests
    {
        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, false)]
        public void GreaterThan_Should_BeValid_When_ValidationIsOK(int value, int other, bool expected)
        {
            var result = Validate(new Fixture { Value = value }, GreaterThan<Fixture, int>(f => f.Value, other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, false)]
        public void GreaterThan_Func_Should_BeValid_When_ValidationIsOK(int value, int other, bool expected)
        {
            var result = Validate(new Fixture { Value = value, OtherValue = other }, GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, false)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        [InlineData(null, null, false)]
        public void GreaterThan_Nullable_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value }, GreaterThan<Fixture, int>(f => f.NullableValue, other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, false)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        [InlineData(null, null, false)]
        public void GreaterThan_NullableFunc_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value, OtherNullableValue = other }, GreaterThan<Fixture, int>(f => f.NullableValue, f => f.OtherNullableValue));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void GreaterThan_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), GreaterThan<Fixture, int>(f => f.Value, 3));
            Assert.Equal(string.Format(GreaterThanMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Func_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            }, GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue));
            Assert.Equal(string.Format(GreaterThanMessage, 10), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Func_Should_UseNullInDefaultMessage_When_NoneIsProvidedAndValueIsNull()
        {
            var result = Validate(new Fixture(), GreaterThan<Fixture, int>(f => f.Value, f => f.NullableValue));
            Assert.Equal(string.Format(GreaterThanMessage, "null"), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), GreaterThan<Fixture, int>(f => f.Value, 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Func_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            }, GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), GreaterThan<Fixture, int>(f => f.Value, 3));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void GreaterThan_Func_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            }, GreaterThan<Fixture, int>(f => f.Value, f => f.NullableValue));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void GreaterThan_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), GreaterThan<Fixture, int>(f => f.Value, 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Fact]
        public void GreaterThan_Func_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            }, GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        class Fixture
        {
            public int Value { get; set; }
            public int OtherValue { get; set; }
            public int? NullableValue { get; set; }
            public int? OtherNullableValue { get; set; }
        }
    }
}
