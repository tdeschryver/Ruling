using System;
using System.Linq;
using Xunit;
using static Ruling.Factory;

namespace Ruling.Tests
{
    public class Ruling_Tests
    {
        [Fact]
        public void Ruling_Should_BeValid_When_ValidationIsOK()
        {
            var ruling = CreateRuling(_fooRequired);
            var result = ruling(new Fixture { Foo = "foo" });

            Assert.True(result.Valid);
        }

        [Fact]
        public void Ruling_Should_HaveNoErrors_When_ValidationIsOK()
        {
            var ruling = CreateRuling(_fooRequired);
            var result = ruling(new Fixture { Foo = "foo" });

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Ruling_Should_BeInvalid_When_ValidationIsNotOK()
        {
            var ruling = CreateRuling(_fooRequired);
            var result = ruling(new Fixture());

            Assert.False(result.Valid);
        }

        [Fact]
        public void Ruling_Should_HaveErrors_When_ValidationIsNotOK()
        {
            var ruling = CreateRuling(_fooRequired);
            var result = ruling(new Fixture());

            Assert.Equal(1, result.Errors.Count);
        }

        [Fact]
        public void Ruling_Should_BeInvalid_When_InputIsNull()
        {
            var ruling = CreateRuling(_fooRequired);
            var result = ruling(null);

            Assert.False(result.Valid);
        }

        [Fact]
        public void Ruling_Should_HaveAKey_When_ValidationIsNotOK()
        {
            var ruling = CreateRuling(_fooRequired);
            var result = ruling(new Fixture());

            Assert.Equal(nameof(Fixture.Foo), result.Errors.Single().Key);
        }

        [Fact]
        public void Ruling_Should_HaveAMessage_When_ValidationIsNotOK()
        {
            var ruling = CreateRuling(_fooRequired);
            var result = ruling(new Fixture());

            Assert.Equal("Foo is required", result.Errors.Single().Value.Single());
        }

        [Fact]
        public void Ruling_Should_HaveAllErrors_When_MultipleRulesAreUsed()
        {
            var ruling = CreateRuling(_fooRequired, _barRequired);
            var result = ruling(new Fixture());

            Assert.Equal(2, result.Errors.Count);
        }

        [Fact]
        public void Ruling_Should_HaveAllKeys_When_MultipleRulesAreUsed()
        {
            var ruling = CreateRuling(_fooRequired, _barRequired);
            var result = ruling(new Fixture());

            Assert.Contains(nameof(Fixture.Foo), result.Errors.Keys);
            Assert.Contains(nameof(Fixture.Bar), result.Errors.Keys);
        }

        [Fact]
        public void Ruling_Should_HaveAllMessages_When_MultipleRulesAreUsed()
        {
            var ruling = CreateRuling(_fooRequired, _barRequired);
            var result = ruling(new Fixture());

            var messages = result.Errors.Values.SelectMany(p => p);
            Assert.Contains("Foo is required", messages);
            Assert.Contains("Bar is required", messages);
        }

        [Fact]
        public void Ruling_Should_HaveAllErrors_When_RulingsAreChained()
        {
            var rulingFoo = CreateRuling(_fooRequired);
            var rulingBar = CreateRuling(_barRequired);
            var ruling = CreateRuling(rulingFoo, rulingBar);
            var result = ruling(new Fixture());

            Assert.Equal(2, result.Errors.Count);
        }

        [Fact]
        public void Ruling_Should_HaveAllKeys_When_RulingsAreChained()
        {
            var rulingFoo = CreateRuling(_fooRequired);
            var rulingBar = CreateRuling(_barRequired);
            var ruling = CreateRuling(rulingFoo, rulingBar);
            var result = ruling(new Fixture());

            Assert.Contains(nameof(Fixture.Foo), result.Errors.Keys);
            Assert.Contains(nameof(Fixture.Bar), result.Errors.Keys);
        }

        [Fact]
        public void Ruling_Should_HaveAllMessages_When_RulingsAreChained()
        {
            var rulingFoo = CreateRuling(_fooRequired);
            var rulingBar = CreateRuling(_barRequired);
            var ruling = CreateRuling(rulingFoo, rulingBar);
            var result = ruling(new Fixture());

            var messages = result.Errors.Values.SelectMany(p => p);
            Assert.Contains("Foo is required", messages);
            Assert.Contains("Bar is required", messages);
        }

        [Fact]
        public void Ruling_Should_StackMessages_When_ItHasTheSameKey()
        {
            var ruling = CreateRuling(_fooRequired, _fooRequired);
            var result = ruling(new Fixture());

            Assert.Equal(1, result.Errors.Keys.Count);
            Assert.Equal(2, result.Errors.Values.SelectMany(p => p).Count());
        }

        Func<Fixture, (bool valid, string key, string message)> _fooRequired =
            f => (!string.IsNullOrWhiteSpace(f.Foo), nameof(Fixture.Foo), "Foo is required");

        Func<Fixture, (bool valid, string key, string message)> _barRequired =
            f => (!string.IsNullOrWhiteSpace(f.Bar), nameof(Fixture.Bar), "Bar is required");

        class Fixture
        {
            public string Foo { get; set; }
            public string Bar { get; set; }
        }
    }
}
