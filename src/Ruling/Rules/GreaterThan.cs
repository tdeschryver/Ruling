using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string GreaterThanMessage = "must be greater than {0}";

        public static Func<TObject, (bool valid, string key, string message)> GreaterThan<TObject, TComparable>(Expression<Func<TObject, IComparable<TComparable>>> selector, IComparable<TComparable> other, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = other == null
                ? string.Format(GreaterThanMessage, "null")
                : GetMessage(message, string.Format(GreaterThanMessage, other));

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (IsInvalidGreaterThan(value, other))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, ruleMessage);
            };
        }

        public static Func<TObject, (bool valid, string key, string message)> GreaterThan<TObject, TComparable>(Expression<Func<TObject, IComparable<TComparable>>> selector, Func<TObject, IComparable<TComparable>> other, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                var otherValue = other?.Invoke(@object);
                var ruleMessage = otherValue == null
                    ? string.Format(GreaterThanMessage, "null")
                    : GetMessage(message, string.Format(GreaterThanMessage, otherValue));

                if (IsInvalidGreaterThan(value, otherValue))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, ruleMessage);
            };
        }

        static bool IsInvalidGreaterThan<TComparable>(IComparable<TComparable> value, IComparable<TComparable> other)
            => value == null || other == null
                ? true
                : value.CompareTo((TComparable)other) <= 0;
    }
}
