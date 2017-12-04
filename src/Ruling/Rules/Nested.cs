using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public static Func<TObject, Result> Nested<TObject, TNestedObject>(Expression<Func<TObject, TNestedObject>> selector, Func<TNestedObject, Result> fun, string message = null, string key = null) where TNestedObject : class
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message);

            return (TObject @object) =>
            {
                var result = new Result();

                var value = selector.Compile().Invoke(@object);
                if (value == null)
                {
                    result.AddError(ruleKey, ruleMessage);
                    return result;
                }

                var nestedResult = fun(value);
                foreach (var error in nestedResult.Errors)
                {
                    result.AddErrors($"{ruleKey}.{error.Key}", error.Value);
                }
                return result;
            };
        }
    }
}
