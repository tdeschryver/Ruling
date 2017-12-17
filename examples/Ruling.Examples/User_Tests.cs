using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Ruling.Factory;
using static Ruling.Validator;
using static Ruling.Rule;

namespace Ruling.Tests
{
    public class User_Tests
    {
        [Fact]
        public void ValidUser()
        {
            var result = Validate(User.CreateValidUser(), Rules);
            Assert.True(result.Valid);
        }

        [Fact]
        public void ValidUser_Ruleset()
        {
            var result = UserOK()(User.CreateValidUser());
            Assert.True(result.Valid);
        }

        [Fact]
        public void InvalidUser()
        {
            var result = Validate(User.CreateInvalidUser(), Rules);
            Assert.False(result.Valid);
            Assert.Contains("Email is incorrect", result.Errors["UserEmail"].Single());
            Assert.Contains("Passwords don't match", result.Errors["PasswordConfirmation"].Single());
        }

        [Fact]
        public void InvalidUser_Ruleset()
        {
            var result = UserOK()(User.CreateInvalidUser());
            Assert.False(result.Valid);
            Assert.Contains("Email is incorrect", result.Errors["UserEmail"].Single());
            Assert.Contains("Passwords don't match", result.Errors["PasswordConfirmation"].Single());
        }

        static Func<User, (bool valid, string key, string message)> EmailRule = Format<User>(u => u.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase, key: "UserEmail", message: "Email is incorrect");
        static Func<User, (bool valid, string key, string message)> PasswordLengthRule = Length<User>(u => u.Password, min: 8);
        static Func<User, (bool valid, string key, string message)> PasswordConfirmationRule = EqualTo<User, string>(u => u.PasswordConfirmation, u => u.Password, message: "Passwords don't match");

        static Func<User, (bool valid, string key, string message)>[] Rules = new[] { EmailRule, PasswordLengthRule, PasswordConfirmationRule };

        static Func<User, Result> UserOK()
        {
            var emailOK = CreateRuling<User>(
             f => Validate(f, EmailRule)
            );
            var passwordOK = CreateRuling<User>(
                f => Validate(f, PasswordLengthRule),
                f => Validate(f, PasswordConfirmationRule)
            );
            return CreateRuling(emailOK, passwordOK);
        }

        class User
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }

            public static User CreateValidUser()
                => new User
                {
                    Email = "foo@bar.baz",
                    Password = "P@ssw0rd",
                    PasswordConfirmation = "P@ssw0rd",
                };

            public static User CreateInvalidUser()
                => new User
                {
                    Email = "foo@bar",
                    Password = "P@ssw0rd",
                    PasswordConfirmation = "password"
                };
        }
    }
}
