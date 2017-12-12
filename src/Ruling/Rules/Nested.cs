using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public static Func<TObject, (bool valid, string key, string message)[]> Nested<TObject, TNestedObject>(Expression<Func<TObject, TNestedObject>> selector, Func<TNestedObject, (bool valid, string key, string message)>[] innerRules, string message = null, string key = null) where TNestedObject : class
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (value == null)
                {
                    return new[] { (false, ruleKey, ruleMessage) };
                }

                return innerRules
                    .Select(rule => rule.Invoke(value))
                    .Select(msg => (msg.valid, $"{ruleKey}.{msg.key}", msg.message))
                    .ToArray();
            };
        }
    }
}
