using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
using static Ruling.Rule;

namespace Ruling.Tests.Rules
{
    public class EqualTo_Tests
    {
        [Theory]
        [InlineData(10, 10, true)]
        [InlineData(-2, 3, false)]
        public void EqualTo_Should_BeValid_When_ValidationIsOK(int value, int other, bool expected)
        {
            var result = Validate(new Fixture { Value = value }, EqualTo<Fixture, int>(f => f.Value, other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 10, true)]
        [InlineData(-2, 3, false)]
        public void EqualTo_Func_Should_BeValid_When_ValidationIsOK(int value, int other, bool expected)
        {
            var result = Validate(new Fixture { Value = value, OtherValue = other }, EqualTo<Fixture, int>(f => f.Value, f => f.OtherValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 10, true)]
        [InlineData(-2, 3, false)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        [InlineData(null, null, false)]
        public void EqualTo_Nullable_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value }, EqualTo<Fixture, int>(f => f.NullableValue, other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 10, true)]
        [InlineData(-2, 3, false)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        [InlineData(null, null, false)]
        public void EqualTo_NullableFunc_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value, OtherNullableValue = other }, EqualTo<Fixture, int>(f => f.NullableValue, f => f.OtherNullableValue));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void EqualTo_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), EqualTo<Fixture, int>(f => f.Value, 3));
            Assert.Equal(string.Format(EqualToMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void EqualTo_Func_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            }, EqualTo<Fixture, int>(f => f.Value, f => f.OtherValue));
            Assert.Equal(string.Format(EqualToMessage, 10), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void EqualTo_Func_Should_UseNullInDefaultMessage_When_NoneIsProvidedAndValueIsNull()
        {
            var result = Validate(new Fixture(), EqualTo<Fixture, int>(f => f.Value, f => f.NullableValue));
            Assert.Equal(string.Format(EqualToMessage, "null"), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void EqualTo_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), EqualTo<Fixture, int>(f => f.Value, 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void EqualTo_Func_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            },
            EqualTo<Fixture, int>(f => f.Value, f => f.OtherValue, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void EqualTo_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), EqualTo<Fixture, int>(f => f.Value, 3));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void EqualTo_Func_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            }, EqualTo<Fixture, int>(f => f.Value, f => f.NullableValue));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void EqualTo_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), EqualTo<Fixture, int>(f => f.Value, 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Fact]
        public void EqualTo_Func_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture
            {
                OtherValue = 10
            }, EqualTo<Fixture, int>(f => f.Value, f => f.OtherValue, key: "Foooo"));
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
