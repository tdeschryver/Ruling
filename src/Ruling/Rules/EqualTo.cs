using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string EqualToMessage = "must be equal to {0}";

        public static Func<TObject, (bool valid, string key, string message)> EqualTo<TObject, TEqual>(Expression<Func<TObject, IEquatable<TEqual>>> selector, IEquatable<TEqual> other, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = other == null
                ? string.Format(EqualToMessage, "null")
                : GetMessage(message, string.Format(EqualToMessage, other));

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (IsInvalidEqualTo(value, other))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, nameof(EqualTo));
            };
        }

        public static Func<TObject, (bool valid, string key, string message)> EqualTo<TObject, TEqual>(Expression<Func<TObject, IEquatable<TEqual>>> selector, Func<TObject, IEquatable<TEqual>> other, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                var otherValue = other?.Invoke(@object);
                var ruleMessage = otherValue == null
                    ? string.Format(EqualToMessage, "null")
                    : GetMessage(message, string.Format(EqualToMessage, otherValue));

                if (IsInvalidEqualTo(value, otherValue))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, nameof(EqualTo));
            };
        }

        static bool IsInvalidEqualTo<TEqual>(IEquatable<TEqual> value, IEquatable<TEqual> other)
            => value == null || other == null
                ? true
                : !value.Equals(other);
    }
}
