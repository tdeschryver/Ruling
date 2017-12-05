using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string MaxLengthMessage = "length must be less than {0}";

        public static Func<TObject, (bool valid, string key, string message)> MaxLength<TObject>(Expression<Func<TObject, string>> selector, int maxLength, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, string.Format(MaxLengthMessage, maxLength));

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (value.Length > maxLength)
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, ruleMessage);
            };
        }
    }
}
