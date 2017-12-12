using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
using static Ruling.Rule;

namespace Ruling.Tests.Rules
{
    public class Required_Tests
    {
        [Theory]
        [InlineData("foo", true)]
        [InlineData("bar", true)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData(null, false)]
        public void Required_String_Should_BeValid_When_ValidationIsOK(string value, bool expected)
        {
            var result = Validate(new Fixture { StringValue = value }, Required<Fixture>(f => f.StringValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(47, true)]
        [InlineData(0, true)]
        [InlineData(-47, true)]
        [InlineData(null, false)]
        public void Required_NullableInt_Should_BeValid_When_ValidationIsOK(int? value, bool expected)
        {
            var result = Validate(new Fixture { NullableIntValue = value }, Required<Fixture>(f => f.NullableIntValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(47, true)]
        [InlineData(0, true)]
        [InlineData(-47, true)]
        [InlineData(null, false)]
        public void Required_NullableInt64_Should_BeValid_When_ValidationIsOK(Int64? value, bool expected)
        {
            var result = Validate(new Fixture { NullableInt64Value = value }, Required<Fixture>(f => f.NullableInt64Value));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(47, true)]
        [InlineData(0, true)]
        [InlineData(-47, true)]
        public void Required_Double_Should_BeValid_When_ValidationIsOK(double value, bool expected)
        {
            var result = Validate(new Fixture { DoubleValue = value }, Required<Fixture>(f => f.DoubleValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(new string[] { "foo" }, true)]
        [InlineData(new string[] { }, false)]
        [InlineData(null, false)]
        public void Required_IEnumerable_Should_BeValid_When_ValidationIsOK(IEnumerable<string> value, bool expected)
        {
            var result = Validate(new Fixture { IEnumerableValue = value }, Required<Fixture>(f => f.IEnumerableValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [MemberData(nameof(DictionaryData))]
        public void Required_Dictionary_Should_BeValid_When_ValidationIsOK(Dictionary<string, string> value, bool expected)
        {
            var result = Validate(new Fixture { DictionaryValue = value }, Required<Fixture>(f => f.DictionaryValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [MemberData(nameof(NestedValueData))]
        public void Required_Nested_Should_BeValid_When_ValidationIsOK(InnerFixture value, bool expected)
        {
            var result = Validate(new Fixture { NestedValue = value }, Required<Fixture>(f => f.NestedValue));
            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [MemberData(nameof(ObjectData))]
        public void Required_Object_Should_BeValid_When_ValidationIsOK(object value, bool expected)
        {
            var result = Validate(new Fixture { ObjectValue = value }, Required<Fixture>(f => f.ObjectValue));
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void Required_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Required<Fixture>(f => f.StringValue));
            Assert.Equal(RequiredMessage, result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Required_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Required<Fixture>(f => f.StringValue, message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Required_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), Required<Fixture>(f => f.StringValue));
            Assert.Equal(nameof(Fixture.StringValue), result.Errors.Single().Key);
        }

        [Fact]
        public void Required_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), Required<Fixture>(f => f.StringValue, key: "Foooo"));
            Assert.Equal("Foooo", result.Errors.Single().Key);
        }

        class Fixture
        {
            public string StringValue { get; set; }
            public int? NullableIntValue { get; set; }
            public Int64? NullableInt64Value { get; set; }
            public double DoubleValue { get; set; }
            public IEnumerable<string> IEnumerableValue { get; set; }
            public Dictionary<string, string> DictionaryValue { get; set; }
            public InnerFixture NestedValue { get; set; }
            public object ObjectValue { get; set; }
        }

        public class InnerFixture
        {
        }

        public static IEnumerable<object[]> DictionaryData => new[]
        {
            new object[] { new Dictionary<string, string>{ ["foo"] = "foo" }, true },
            new object[] { new Dictionary<string, string>(), false },
            new object[] { null, false },
        };

        public static IEnumerable<object[]> NestedValueData => new[]
        {
            new object[] { new InnerFixture(), true },
            new object[] { null, false },
        };

        public static IEnumerable<object[]> ObjectData => new[]
        {
            new object[] { new { Foo = "foo" }, true },
            new object[] { "foo", true },
            new object[] { 2, true },
            new object[] { 0, true },
            new object[] { new int[]{ 1, 2, 3 }, true },
            new object[] { new Dictionary<string, string>{ ["foo"] = "foo" }, true },

            new object[] { null, false },
            new object[] { "", false },
            new object[] { " ", false },
            new object[] { new int[]{}, false },
            new object[] { new Dictionary<string, string>(), false },
        };
    }
}
