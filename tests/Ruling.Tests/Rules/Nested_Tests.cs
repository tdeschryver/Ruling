using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Factory;
using static Ruling.Rule;

namespace Ruling.Tests.Rules
{
    public class Nested_Tests
    {
        [Theory]
        [InlineData("foo", true)]
        [InlineData("", false)]
        public void Nested_Should_BeValid_When_NestedValidationIsOK(string value, bool expected)
        {
            var ruling = CreateRuling(NestedRuling());
            var result = ruling(new Fixture
            {
                NestedValue = new InnerFixture
                {
                    StringValue = value
                }
            });

            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void Nested_Should_BeInvalid_When_NestedObjectIsNull()
        {
            var ruling = CreateRuling(NestedRuling());
            var result = ruling(new Fixture());

            Assert.False(result.Valid);
        }

        [Fact]
        public void Nested_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var ruling = CreateRuling(NestedRuling());
            var result = ruling(new Fixture());

            Assert.Equal(DefaultInvalidMessage, result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Nested_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var ruling = CreateRuling(NestedRuling(message: "Custom message"));
            var result = ruling(new Fixture());

            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Nested_Should_UsePropertyName_When_NoneIsProvided()
        {
            var ruling = CreateRuling(NestedRuling());
            var result = ruling(new Fixture());

            Assert.Equal(nameof(Fixture.NestedValue), result.Errors.Single().Key);
        }

        [Fact]
        public void Nested_Should_OverridePropertyName_When_OneIsProvided()
        {
            var ruling = CreateRuling(NestedRuling(key: "Nested"));
            var result = ruling(new Fixture());

            Assert.Equal("Nested", result.Errors.Single().Key);
        }

        [Fact]
        public void Nested_Should_ChainKeys_When_NestedObjectPropertyIsInvalid()
        {
            var ruling = CreateRuling(NestedRuling());
            var result = ruling(new Fixture
            {
                NestedValue = new InnerFixture()
            });

            Assert.Equal("NestedValue.StringValue", result.Errors.Single().Key);
        }

        [Fact]
        public void Nested_Should_ShowMessage_When_NestedObjectPropertyIsInvalid()
        {
            var ruling = CreateRuling(NestedRuling());
            var result = ruling(new Fixture
            {
                NestedValue = new InnerFixture()
            });

            Assert.Equal(RequiredMessage, result.Errors.Single().Value.Single());
        }

        Func<InnerFixture, Result> InnerRuling => CreateRuling(Required<InnerFixture>(f => f.StringValue));

        Func<Fixture, Result> NestedRuling(string message = null, string key = null)
            => CreateRuling(Nested<Fixture, InnerFixture>(f => f.NestedValue, InnerRuling, message, key));

        class Fixture
        {
            public InnerFixture NestedValue { get; set; }
        }

        public class InnerFixture
        {
            public string StringValue { get; set; }
        }
    }
}
