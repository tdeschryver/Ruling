using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Factory;
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
            var ruling = CreateRuling(Required<Fixture>(f => f.StringValue));
            var result = ruling(new Fixture { StringValue = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(47, true)]
        [InlineData(0, true)]
        [InlineData(-47, true)]
        [InlineData(null, false)]
        public void Required_NullableInt_Should_BeValid_When_ValidationIsOK(int? value, bool expected)
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.NullableIntValue));
            var result = ruling(new Fixture { NullableIntValue = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(47, true)]
        [InlineData(0, true)]
        [InlineData(-47, true)]
        [InlineData(null, false)]
        public void Required_NullableInt64_Should_BeValid_When_ValidationIsOK(Int64? value, bool expected)
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.NullableInt64Value));
            var result = ruling(new Fixture { NullableInt64Value = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(47, true)]
        [InlineData(0, true)]
        [InlineData(-47, true)]
        public void Required_Double_Should_BeValid_When_ValidationIsOK(double value, bool expected)
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.DoubleValue));
            var result = ruling(new Fixture { DoubleValue = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [InlineData(new string[] { "foo" }, true)]
        [InlineData(new string[] { }, false)]
        [InlineData(null, false)]
        public void Required_IEnumerable_Should_BeValid_When_ValidationIsOK(IEnumerable<string> value, bool expected)
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.IEnumerableValue));
            var result = ruling(new Fixture { IEnumerableValue = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [MemberData(nameof(DictionaryData))]
        public void Required_Dictionary_Should_BeValid_When_ValidationIsOK(Dictionary<string, string> value, bool expected)
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.DictionaryValue));
            var result = ruling(new Fixture { DictionaryValue = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [MemberData(nameof(NestedValueData))]
        public void Required_Nested_Should_BeValid_When_ValidationIsOK(InnerFixture value, bool expected)
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.NestedValue));
            var result = ruling(new Fixture { NestedValue = value });

            Assert.Equal(expected, result.Valid);
        }

        [Theory]
        [MemberData(nameof(ObjectData))]
        public void Required_Object_Should_BeValid_When_ValidationIsOK(object value, bool expected)
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.ObjectValue));
            var result = ruling(new Fixture { ObjectValue = value });

            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void Required_Should_BeInvalid_When_InputIsNull()
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.StringValue));
            var result = ruling(null);

            Assert.False(result.Valid);
        }

        [Fact]
        public void Required_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.StringValue));
            var result = ruling(new Fixture());

            Assert.Equal(RequiredMessage, result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Required_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.StringValue, message: "Custom message"));
            var result = ruling(new Fixture());

            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Required_Should_UsePropertyName_When_NoneIsProvided()
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.StringValue));
            var result = ruling(new Fixture());

            Assert.Equal(nameof(Fixture.StringValue), result.Errors.Single().Key);
        }

        [Fact]
        public void Required_Should_OverridePropertyName_When_OneIsProvided()
        {
            var ruling = CreateRuling(Required<Fixture>(f => f.StringValue, key: "Foooo"));
            var result = ruling(new Fixture());

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
