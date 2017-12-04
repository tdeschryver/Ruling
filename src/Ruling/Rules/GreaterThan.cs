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

        public static Func<TObject, (bool valid, string key, string message)> GreaterThan<TObject, TProperty>(Expression<Func<TObject, TProperty>> selector, TProperty other, string message = null, string key = null) where TProperty : IComparable<TProperty>
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, string.Format(GreaterThanMessage, other));

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (value.CompareTo(other) > 0)
                {
                    return (true, ruleKey, ruleMessage);
                }

                return (false, ruleKey, ruleMessage);
            };
        }

        public static Func<TObject, (bool valid, string key, string message)> GreaterThan<TObject>(Expression<Func<TObject, object>> selector, object other, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, string.Format(GreaterThanMessage, other ?? "null"));

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object) as IComparable;
                if (value == null)
                {
                    return (false, ruleKey, ruleMessage);
                }

                var otherValue = other as IComparable;
                if (otherValue == null)
                {
                    return (false, ruleKey, GetMessage(message, string.Format(GreaterThanMessage, "null")));
                }

                if (value?.CompareTo(otherValue) > 0)
                {
                    return (true, ruleKey, ruleMessage);
                }

                return (false, ruleKey, ruleMessage);
            };
        }
    }
}
