using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Ruling.Validator;
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
            var result = Validate(new Fixture
            {
                NestedValue = new InnerFixture
                {
                    StringValue = value
                }
            },
            NestedRules());
            Assert.Equal(expected, result.Valid);
        }

        [Fact]
        public void Nested_Should_BeInvalid_When_NestedObjectIsNull()
        {
            var result = Validate(new Fixture(), NestedRules());
            Assert.False(result.Valid);
        }

        [Fact]
        public void Nested_Should_UseDefaultMessage_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), NestedRules());
            Assert.Equal(DefaultInvalidMessage, result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Nested_Should_OverrideDefaultMessage_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), NestedRules(message: "Custom message"));
            Assert.Equal("Custom message", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Nested_Should_UsePropertyName_When_NoneIsProvided()
        {
            var result = Validate(new Fixture(), NestedRules());
            Assert.Equal(nameof(Fixture.NestedValue), result.Errors.Single().Key);
        }

        [Fact]
        public void Nested_Should_OverridePropertyName_When_OneIsProvided()
        {
            var result = Validate(new Fixture(), NestedRules(key: "Nested"));
            Assert.Equal("Nested", result.Errors.Single().Key);
        }

        [Fact]
        public void Nested_Should_ChainKeys_When_NestedObjectPropertyIsInvalid()
        {
            var result = Validate(new Fixture
            {
                NestedValue = new InnerFixture()
            }, NestedRules());
            Assert.Equal("NestedValue.StringValue", result.Errors.Single().Key);
        }

        [Fact]
        public void Nested_Should_ShowMessage_When_NestedObjectPropertyIsInvalid()
        {
            var result = Validate(new Fixture
            {
                NestedValue = new InnerFixture()
            }, NestedRules());
            Assert.Equal(RequiredMessage, result.Errors.Single().Value.Single());
        }

        Func<Fixture, (bool valid, string key, string message)[]> NestedRules(string message = null, string key = null)
            => Nested<Fixture, InnerFixture>(
                f => f.NestedValue,
                new[] { Required<InnerFixture>(f => f.StringValue) },
                message,
                key
            );

        class Fixture
        {
            public string Foo { get; set; }
            public InnerFixture NestedValue { get; set; }
        }

        public class InnerFixture
        {
            public string StringValue { get; set; }
        }
    }
}
