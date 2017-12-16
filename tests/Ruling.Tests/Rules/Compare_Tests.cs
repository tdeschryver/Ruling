using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
using static Ruling.Rule;

namespace Ruling.Tests.Rules
{
    public class Compare_Tests
    {
        [Fact]
        public void Compare_Should_ThrowArgumentException_When_NoComparerIsProvided()
        {
            int? other = null;
            Assert.Throws<ArgumentException>(() => Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThan: other)));
        }

        [Fact]
        public void Compare_Func_Should_ThrowArgumentException_When_NoComparerIsProvided()
        {
            Func<Fixture, IComparable<int>> other = null;
            Assert.Throws<ArgumentException>(() => Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThan: other)));
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, false)]
        [InlineData(null, 7, false)]
        public void CompareGreaterThan_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value }, Compare<Fixture, int>(f => f.NullableValue, greaterThan: other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, false)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        public void CompareGreaterThan_Func_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value, OtherNullableValue = other }, Compare<Fixture, int>(f => f.NullableValue, greaterThan: f => f.OtherNullableValue));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void CompareGreaterThan_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThan: 3));
            Assert.Equal(string.Format(GreaterThanMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThan_Func_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThan: f => f.OtherValue));
            Assert.Equal(string.Format(GreaterThanMessage, 10), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThan_Func_Should_UseNullInDefaultMessage_When_NoneIsProvidedAndValueIsNull()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThan: f => f.NullableValue));
            Assert.Equal(string.Format(GreaterThanMessage, "null"), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThan_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThan: 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThan_Func_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThan: f => f.OtherValue, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThan_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThan: 3));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareGreaterThan_Func_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThan: f => f.NullableValue));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareGreaterThan_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThan: 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Fact]
        public void CompareGreaterThan_Func_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThan: f => f.OtherValue, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, true)]
        [InlineData(null, 7, false)]
        public void CompareGreaterThanOrEqualTo_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value }, Compare<Fixture, int>(f => f.NullableValue, greaterThanOrEqualTo: other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, true)]
        [InlineData(-2, 3, false)]
        [InlineData(8, 8, true)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        public void CompareGreaterThanOrEqualTo_Func_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value, OtherNullableValue = other }, Compare<Fixture, int>(f => f.NullableValue, greaterThanOrEqualTo: f => f.OtherNullableValue));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: 3));
            Assert.Equal(string.Format(GreaterThanOrEqualToMessage, 3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Func_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: f => f.OtherValue));
            Assert.Equal(string.Format(GreaterThanOrEqualToMessage, 10), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Func_Should_UseNullInDefaultMessage_When_NoneIsProvidedAndValueIsNull()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: f => f.NullableValue));
            Assert.Equal(string.Format(GreaterThanOrEqualToMessage, "null"), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: 3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Func_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: f => f.OtherValue, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: 3));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Func_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: f => f.NullableValue));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: 3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Fact]
        public void CompareGreaterThanOrEqualTo_Func_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = 10 }, Compare<Fixture, int>(f => f.Value, greaterThanOrEqualTo: f => f.OtherValue, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]
        [InlineData(10, 2, false)]
        [InlineData(-2, 3, true)]
        [InlineData(8, 8, false)]
        [InlineData(null, 7, false)]
        public void CompareLessThan_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value }, Compare<Fixture, int>(f => f.NullableValue, lessThan: other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, false)]
        [InlineData(-2, 3, true)]
        [InlineData(8, 8, false)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        public void CompareLessThan_Func_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value, OtherNullableValue = other }, Compare<Fixture, int>(f => f.NullableValue, lessThan: f => f.OtherNullableValue));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void CompareLessThan_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThan: -3));
            Assert.Equal(string.Format(LessThanMessage, -3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThan_Func_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThan: f => f.OtherValue));
            Assert.Equal(string.Format(LessThanMessage, -10), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThan_Func_Should_UseNullInDefaultMessage_When_NoneIsProvidedAndValueIsNull()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThan: f => f.NullableValue));
            Assert.Equal(string.Format(LessThanMessage, "null"), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThan_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThan: -3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThan_Func_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThan: f => f.OtherValue, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThan_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThan: -3));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareLessThan_Func_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThan: f => f.NullableValue));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareLessThan_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThan: -3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Fact]
        public void CompareLessThan_Func_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThan: f => f.OtherValue, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Theory]
        [InlineData(10, 2, false)]
        [InlineData(-2, 3, true)]
        [InlineData(8, 8, true)]
        [InlineData(null, 7, false)]
        public void CompareLessThanOrEqualTo_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value }, Compare<Fixture, int>(f => f.NullableValue, lessThanOrEqualTo: other));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(10, 2, false)]
        [InlineData(-2, 3, true)]
        [InlineData(8, 8, true)]
        [InlineData(null, 7, false)]
        [InlineData(7, null, false)]
        public void CompareLessThanOrEqualTo_Func_Should_BeValid_When_ValidationIsOK(int? value, int? other, bool expected)
        {
            var result = Validate(new Fixture { NullableValue = value, OtherNullableValue = other }, Compare<Fixture, int>(f => f.NullableValue, lessThanOrEqualTo: f => f.OtherNullableValue));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: -3));
            Assert.Equal(string.Format(LessThanOrEqualToMessage, -3), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Func_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: f => f.OtherValue));
            Assert.Equal(string.Format(LessThanOrEqualToMessage, -10), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Func_Should_UseNullInDefaultMessage_When_NoneIsProvidedAndValueIsNull()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: f => f.NullableValue));
            Assert.Equal(string.Format(LessThanOrEqualToMessage, "null"), result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: -3, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Func_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: f => f.OtherValue, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: -3));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Func_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: f => f.NullableValue));
            Assert.Equal(nameof(Fixture.Value), result.Errors.Single().Key);
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: -3, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        [Fact]
        public void CompareLessThanOrEqualTo_Func_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture { OtherValue = -10 }, Compare<Fixture, int>(f => f.Value, lessThanOrEqualTo: f => f.OtherValue, key: "Foooo"));
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
