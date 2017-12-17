using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string FormatMessage = "has invalid format";

        public static Func<TObject, (bool valid, string key, string message)> Format<TObject>(Expression<Func<TObject, string>> selector, string pattern, RegexOptions options = RegexOptions.None, string message = null, string key = null)
        {
            var ruleKey = GetKey(selector, key);
            var ruleMessage = GetMessage(message, FormatMessage);

            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);
                if (IsInvalidFormat(value, pattern, options))
                {
                    return (false, ruleKey, ruleMessage);
                }

                return (true, ruleKey, nameof(Format));
            };
        }

        static bool IsInvalidFormat(string value, string pattern, RegexOptions options)
            => value == null || pattern == null
                ? true
                : !Regex.IsMatch(value, pattern, options);
    }
}
