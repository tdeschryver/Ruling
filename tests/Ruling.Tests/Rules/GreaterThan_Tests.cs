using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Factory;
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
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, other));
            var result = ruling(new Fixture { Value = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, false)]
        public void GreaterThan_Func_Should_BeValid_When_ValidationIsOK(int value, int other, bool expected)
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue));
            var result = ruling(new Fixture { Value = value, OtherValue = other });

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
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.NullableValue, other));
            var result = ruling(new Fixture { NullableValue = value });

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
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.NullableValue, f => f.OtherNullableValue));
            var result = ruling(new Fixture { NullableValue = value, OtherNullableValue = other });

            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void GreaterThan_Should_BeInvalid_When_InputIsNull()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, 3));
            var result = ruling(null);

            Assert.False(result.Valid);
        }

        [Fact]
        public void GreaterThan_Func_Should_BeInvalid_When_InputIsNull()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue));
            var result = ruling(null);

            Assert.False(result.Valid);
        }

        [Fact]
        public void GreaterThan_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, 3));
            var result = ruling(new Fixture());

            Assert.Equal(string.Format(GreaterThanMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Func_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue));
            var result = ruling(new Fixture
            {
                OtherValue = 10
            });

            Assert.Equal(string.Format(GreaterThanMessage, 10), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Func_Should_UseNullInDefaultMessage_When_NoneIsProvidedAndValueIsNull()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, f => f.NullableValue));
            var result = ruling(new Fixture());

            Assert.Equal(string.Format(GreaterThanMessage, "null"), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, 3, message: "Custom message"));
            var result = ruling(new Fixture());

            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Func_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue, message: "Custom message"));
            var result = ruling(new Fixture
            {
                OtherValue = 10
            });

            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void GreaterThan_Should_UsePropertyName_When_NoneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, 3));
            var result = ruling(new Fixture());

            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void GreaterThan_Func_Should_UsePropertyName_When_NoneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, f => f.NullableValue));
            var result = ruling(new Fixture
            {
                OtherValue = 10
            });

            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void GreaterThan_Should_OverridePropertyName_When_OneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, 3, key: "Foooo"));
            var result = ruling(new Fixture());

            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Fact]
        public void GreaterThan_Func_Should_OverridePropertyName_When_OneIsProvided()
        {
            var ruling = CreateRuling(GreaterThan<Fixture, int>(f => f.Value, f => f.OtherValue, key: "Foooo"));
            var result = ruling(new Fixture
            {
                OtherValue = 10
            });

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
