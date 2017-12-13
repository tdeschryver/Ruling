using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string RequiredMessage = "is required";

        public static Func<TObject, (bool valid, string key, string message)> Required<TObject>(Expression<Func<TObject, object>> selector, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, RequiredMessage);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (value == null)
                {
                    return (false, ruleKey, ruleMessage);
                }
                else if (value is string && IsInvalidRequiredString((string)value))
                {
                    return (false, ruleKey, ruleMessage);
                }
                else if (value is ICollection && IsInvalidRequiredICollection((ICollection)value))
                {
                    return (false, ruleKey, ruleMessage);
                }
                else if (value is IEnumerable<object> && IsInvalidRequiredIEnumerable((IEnumerable<object>)value))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, nameof(Required));
            };
        }

        public static Func<TObject, (bool valid, string key, string message)> Required<TObject>(Expression<Func<TObject, string>> selector, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, RequiredMessage);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (value == null || IsInvalidRequiredString(value))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, nameof(Required));
            };
        }

        public static Func<TObject, (bool valid, string key, string message)> Required<TObject>(Expression<Func<TObject, IEnumerable<object>>> selector, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, RequiredMessage);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (value == null || IsInvalidRequiredIEnumerable(value))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, nameof(Required));
            };
        }

        public static Func<TObject, (bool valid, string key, string message)> Required<TObject>(Expression<Func<TObject, ICollection>> selector, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, RequiredMessage);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (value == null || IsInvalidRequiredICollection(value))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, nameof(Required));
            };
        }

        static bool IsInvalidRequiredString(string value) => string.IsNullOrWhiteSpace(value);
        static bool IsInvalidRequiredIEnumerable(IEnumerable<object> value) => !value.Any();
        static bool IsInvalidRequiredICollection(ICollection value) => value.Count == 0;
    }
}
